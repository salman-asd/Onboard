using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Application.Common.Models;
using ASD.Onboard.Application.Features.Identity.Commands;
using ASD.Onboard.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ASD.Onboard.Infrastructure.Identity;

public class IdentityService(
    UserManager<AppUser> userManager,
    IConfiguration configuration,
    IEmailOutboxRepository emailOutboxRepository,
    ITokenEncrypDecryptService tokenEncrypDecryptService) : IIdentityService
{
    public async Task<string?> GetUserNameAsync(string userId)
    {
        Guard.Against.NullOrEmpty(userId, nameof(userId));
        var user = await userManager.FindByIdAsync(userId);

        return user?.UserName;
    }

    public async Task<Result> CreateUserAsync(UserRegisterCommand request)
    {
        Guard.Against.Null(request, nameof(request));
        Guard.Against.NullOrEmpty(request.Email, nameof(request.Email));
        Guard.Against.NullOrEmpty(request.Password, nameof(request.Password));
        Guard.Against.NullOrEmpty(request.FirstName, nameof(request.FirstName));
        Guard.Against.NullOrEmpty(request.LastName, nameof(request.LastName));

        var user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNo,
            UserName = request.Email,
            Email = request.Email
        };
        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            // Send confirmation email
            await QueueConfirmationEmailAsync(request.Email);
        }

        return result.ToApplicationResult();
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        Guard.Against.NullOrEmpty(userId, nameof(userId));

        var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(AppUser user)
    {
        Guard.Against.Null(user, nameof(user));

        var result = await userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    public async Task<Result> QueueConfirmationEmailAsync(string email)
    {
        try
        {
            Guard.Against.NullOrEmpty(email, nameof(email));

            var user = await userManager.FindByEmailAsync(email);
            
            Guard.Against.NotFound(email, user);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encryptedToken = tokenEncrypDecryptService.EncryptToken(token);
            var confirmLink = BuildConfirmationLink(email, encryptedToken);
            var emailMessage = BuildConfirmationEmail(email, confirmLink);

            await emailOutboxRepository.AddAsync(emailMessage);
            return Result.Success();
        }
        catch (NotFoundException ex)
        {
            return Result.Failure([$"Failed to queue confirmation email: {ex.Message}"]);
        }
        catch (Exception ex)
        {
            // Log the error here
            return Result.Failure([$"An unexpected error occurred while queueing confirmation email"]);
        }
    }

    private string BuildConfirmationLink(string email, string token)
    {
        var apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl")
            ?? throw new InvalidOperationException("ApiBaseUrl configuration is missing");
        var apiConfirmEmail = configuration.GetValue<string>("ApiConfirmEmail")
            ?? throw new InvalidOperationException("ApiConfirmEmail configuration is missing");

        return $"{apiBaseUrl.TrimEnd('/')}{apiConfirmEmail}?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";
    }

    private EmailOutboxMessage BuildConfirmationEmail(string email, string confirmLink)
    {
        var expiryHours = configuration.GetValue<int>("EmailConfirmationTokenExpiryHours", 24);
        var subject = "Confirm Your Email";
        var body = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <h2>Thank you for registering!</h2>
            <p>Please confirm your email address by clicking the button below:</p>
            <div style='text-align: center; margin: 25px 0;'>
                <a href='{confirmLink}' 
                   style='background-color: #4CAF50; color: white; padding: 12px 25px; 
                          text-decoration: none; border-radius: 4px; display: inline-block;'>
                    Confirm Email
                </a>
            </div>
            <p>Or copy and paste this link into your browser:</p>
            <p style='word-break: break-all;'>{confirmLink}</p>
            <p>If you did not request this, please ignore this email.</p>
            <p><strong>This link will expire in {expiryHours} hours.</strong></p>
        </div>";

        return EmailOutboxMessage.CreateHtmlEmail(
            to: email,
            subject: subject,
            htmlBody: body);
    }
}
