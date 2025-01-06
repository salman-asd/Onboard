using ASD.Onboard.Application.Common.Extensions;
using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Application.Common.Models;
using ASD.Onboard.Application.Features.Identity.Models;
using ASD.Onboard.Infrastructure.Identity.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ASD.Onboard.Infrastructure.Identity;

internal sealed class AuthService(
    UserManager<AppUser> userManager,
    IOptions<JwtSettings> jwtSettings,
    ITokenProvider tokenProvider,
    IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory,
    IAuthorizationService authorizationService,
    IBackgroundTaskQueue backgroundTaskQueue,
    IEmailService emailService,
    IConfiguration configuration,
    ITokenEncrypDecryptService tokenEncrypDecryptService) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<AuthResponse> LoginAsync(string username, string password)
    {
        Guard.Against.NullOrEmpty(username, nameof(username));
        Guard.Against.NullOrEmpty(password, nameof(password));

        var user = await userManager.FindByEmailAsync(username);

        if (user is null || !await userManager.CheckPasswordAsync(user, password))
            Guard.Against.NotFound(username, username, "User not found or invalid credentials.");

        if (!user.EmailConfirmed)
            Guard.Against.EmailNotConfirmed(user.EmailConfirmed, nameof(user.EmailConfirmed));

        var roles = await userManager.GetRolesAsync(user);

        var token = tokenProvider.GenerateAccessToken(user.Id, user.Email, roles);

        return new AuthResponse(token);
    }

    public async Task<Result> ForgotPasswordAsync(string email)
    {
        // Find the user by email
        var user = await userManager.FindByEmailAsync(email);

        // Check if user exists
        if (user == null)
            throw new Exception("User not found.");

        // Optional: Ensure the user's email is confirmed
        if (!user.EmailConfirmed)
            throw new Exception("Email not confirmed.");

        // Generate password reset token
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        // Encode the token for safe transmission in a URL
        var encodedToken = Uri.EscapeDataString(token);

        // Construct the password reset link
        var resetLink = $"https://yourapp.com/reset-password?email={Uri.EscapeDataString(email)}&token={encodedToken}";

        // Queue the job to send the password reset email with retry mechanism
        await backgroundTaskQueue.EnqueueWithRetry(async cancellationToken =>
        {
            await emailService.SendHtmlEmailAsync(
                email,
                "Password Reset Request",
                $"We received a request to reset your password. Click the link to reset your password: {resetLink}. If you did not request this, please ignore this email."
            );
        }, 3);


        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            Guard.Against.NotFound(userId, user);

        var identityResult = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        return identityResult.ToApplicationResult();
    }

    public async Task<Result> ConfirmEmailAsync(string email, string token, CancellationToken cancellationToken = default)
    {
        try
        {
            Guard.Against.NullOrWhiteSpace(email, nameof(email));
            Guard.Against.NullOrWhiteSpace(token, nameof(token));

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure(["User not found"]);

            if (user.EmailConfirmed)
                return Result.Failure(["Email already confirmed"]);


            var decryptedToken = tokenEncrypDecryptService.DecryptToken(token);
            // Token validation will automatically check expiration
            var result = await userManager.ConfirmEmailAsync(user, decryptedToken);
            if (!result.Succeeded)
            {
                // Check if token is expired
                if (result.Errors.Any(e => e.Code == "InvalidToken"))
                {
                    return Result.Failure(["Confirmation link has expired. Please request a new one."]);
                }
                return Result.Failure(result.Errors.Select(e => e.Description).ToArray());
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure([$"Email confirmation failed: {ex.Message}"]);
        }
    }

    public async Task<Result> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
            throw new Exception("User not found.");

        var result = await userManager.ResetPasswordAsync(user, token, newPassword);

        return result.ToApplicationResult();
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await userClaimsPrincipalFactory.CreateAsync(user);

        var result = await authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }
}
