using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Application.Common.Models;
using ASD.Onboard.Application.Features.Identity.Models;
using ASD.Onboard.Infrastructure.Identity.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ASD.Onboard.Infrastructure.Identity;

internal sealed class AuthService(
    UserManager<AppUser> userManager,
    IOptions<JwtSettings> jwtSettings,
    ITokenProvider tokenProvider,
    IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory,
    IAuthorizationService authorizationService,
    IBackgroundTaskQueue backgroundTaskQueue,
    IEmailService emailService) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<AuthResponse> LoginAsync(string username, string password)
    {
        var user = await userManager.FindByEmailAsync(username);

        if (user == null || !await userManager.CheckPasswordAsync(user, password))
            throw new Exception("Invalid credentials.");

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
            await emailService.SendHtmlEmail(
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
            throw new Exception("User not found.");

        var identityResult = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        return identityResult.ToApplicationResult();
    }

    public async Task<Result> ConfirmEmailAsync(string email, string token)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
            throw new Exception("User not found.");

        var result = await userManager.ConfirmEmailAsync(user, token);

        return result.ToApplicationResult();
    }

    public async Task<Result> SendConfirmationEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
            throw new Exception("User not found.");

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmLink = $"https://yourapp.com/confirm-email?email={email}&token={Uri.EscapeDataString(token)}";

        // Queue the task to send the confirmation email with retry mechanism
        await backgroundTaskQueue.EnqueueWithRetry(async cancellationToken =>
        {
            await emailService.SendEmail(
                email,
                "Confirm Your Email",
                $"Click the link to confirm your email: {confirmLink}"
            );
        }, 3);

        return Result.Success();
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
