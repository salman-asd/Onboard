using System.Text;
using ASD.Onboard.Application.Common.Extensions;
using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Application.Common.Models;
using ASD.Onboard.Application.Features.Identity.Models;
using ASD.Onboard.Infrastructure.EmailCommnunication;
using ASD.Onboard.Infrastructure.Identity.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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

        Guard.Against.InvalidUserCredential(user);

        Guard.Against.InvalidCredential(await userManager.CheckPasswordAsync(user, password));

        if (!user.EmailConfirmed)
            Guard.Against.EmailNotConfirmed(user.EmailConfirmed, nameof(user.EmailConfirmed));

        var roles = await userManager.GetRolesAsync(user);

        var token = tokenProvider.GenerateAccessToken(user.Id, user.Email, roles);

        return new AuthResponse(token);
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

    public async Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            Guard.Against.NotFound(userId, user);

        var identityResult = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        return identityResult.ToApplicationResult();
    }


    public async Task<Result> ForgotPasswordAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
            Guard.Against.InvalidUserCredential(user);

        if (!user.EmailConfirmed)
            Guard.Against.EmailNotConfirmed(user.EmailConfirmed, nameof(user.EmailConfirmed));

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(email));

        var clientBaseUrl = configuration["ClientBaseUrl"];
        var resetUrl = configuration["ResetUrl"];
        var resetLink = $"{clientBaseUrl}{resetUrl}?email={encodedEmail}&token={encodedToken}";

        // Queue the job to send the password reset email with retry mechanism
        await backgroundTaskQueue.EnqueueWithRetry(async cancellationToken =>
        {
            var emailTemplate = @$"
                    <h2>Password Reset Request</h2>
                    <p>We received a request to reset your password.</p>
                    <p>Click the link below to reset your password:</p>
                    <a href='{resetLink}'>Reset Password</a>
                    <p>If you did not request this, please ignore this email.</p>
                    <p>This link will expire in 24 hours.</p>";

            await emailService.SendHtmlEmailAsync(
                email,
                "Password Reset Request",
                emailTemplate
            );
        }, 3);


        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(string email, string token, string newPassword)
    {
        try
        {
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var decodedEmail = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(email));

            var user = await userManager.FindByEmailAsync(decodedEmail);

            if (user is null)
                Guard.Against.InvalidUserCredential(user);

            var result = await userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            return result.ToApplicationResult();
        }
        catch (Exception ex)
        {
            return Result.Failure([$"An error occurred: {ex.Message}"]);
        }
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
