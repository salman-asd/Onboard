using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Application.Common.Interfaces.Identity;
using ASD.Onboard.Application.Common.Models;
using ASD.Onboard.Domain.Entities;
using ASD.Onboard.Domain.Entities.Applicants;
using ASD.Onboard.Infrastructure.Data;
using ASD.Onboard.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASD.Onboard.Infrastructure.EmailCommnunication;

internal sealed class EmailConfirmationService(
    ApplicationDbContext context,
    IEmailOutboxRepository emailOutboxRepository,
    IConfiguration configuration,
    ILogger<EmailConfirmationService> logger,
    UserManager<AppUser> userManager) : IEmailConfirmationService
{
    public async Task<Result> SendConfirmationEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            Guard.Against.NullOrEmpty(email, nameof(email));

            // Find user without UserManager
            var user = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            Guard.Against.NotFound(email, user);

            if (user.EmailConfirmed)
            {
                return Result.Failure(["Email is already confirmed"]);
            }

            // Create token entity
            var tokenLifetime = TimeSpan.FromHours(
                configuration.GetValue<int>("EmailConfirmationTokenExpiryHours", 24));

            var token = EmailConfirmationToken.Create(user.Id, tokenLifetime);

            context.EmailConfirmationTokens.Add(token);

            // Build confirmation email
            var confirmLink = BuildConfirmationLink(email, token.Id.ToString());
            var emailMessage = BuildConfirmationEmail(email, confirmLink);

            // Use transaction to ensure both operations succeed
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await context.SaveChangesAsync(cancellationToken);
                await emailOutboxRepository.AddAsync(emailMessage, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            return Result.Success();
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning(ex, "User not found during email confirmation send attempt");
            return Result.Failure([$"Failed to send confirmation email: {ex.Message}"]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending confirmation email");
            return Result.Failure(["An unexpected error occurred while sending confirmation email"]);
        }
    }

    public async Task<Result> ConfirmEmailAsync(string email, Guid tokenId, CancellationToken cancellationToken = default)
    {
        try
        {
            Guard.Against.NullOrEmpty(email, nameof(email));
            Guard.Against.Default(tokenId, nameof(tokenId));

            // Use a single query to get both user and token
            var userAndToken = await context.Users
                .Where(u => u.Email == email)
                .Select(u => new
                {
                    User = u,
                    Token = context.EmailConfirmationTokens
                        .FirstOrDefault(t => t.Id == tokenId && t.UserId == u.Id)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (userAndToken?.User == null)
                return Result.Failure(["User not found"]);

            if (userAndToken.User.EmailConfirmed)
                return Result.Failure(["Email is already confirmed"]);

            if (userAndToken.Token == null)
                return Result.Failure(["Invalid confirmation token"]);

            if (userAndToken.Token.IsUsed)
                return Result.Failure(["This confirmation link has already been used"]);

            if (userAndToken.Token.ExpiryTime < DateTime.Now)
                return Result.Failure(["The confirmation link has expired. Please request a new one"]);

            // Update user and token in a single transaction
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                userAndToken.User.EmailConfirmed = true;
                userAndToken.Token.MarkAsUsed();

                await context.SaveChangesAsync(cancellationToken);

                await userManager.UpdateSecurityStampAsync(userAndToken.User);
                // Create applicant profile
                await CreateApplicant(
                    userAndToken.User.Id,
                    userAndToken.User.FirstName,
                    userAndToken.User.LastName,
                    email,
                    cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return Result.Success();
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error confirming email for {Email}", email);
            return Result.Failure([$"Email confirmation failed: {ex.Message}"]);
        }
    }

    private string BuildConfirmationLink(string email, string tokenId)
    {
        var apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl")
            ?? throw new InvalidOperationException("ApiBaseUrl configuration is missing");
        var apiConfirmEmail = configuration.GetValue<string>("ApiConfirmEmail")
            ?? throw new InvalidOperationException("ApiConfirmEmail configuration is missing");

        return $"{apiBaseUrl.TrimEnd('/')}{apiConfirmEmail}?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(tokenId)}";
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

    private async Task CreateApplicant(string userId, string firstName, string lastName, string email,
        CancellationToken cancellationToken)
    {
        try
        {
            context.Applicants.Add(Applicant.Create(userId, firstName, lastName, email));
            await context.SaveChangesAsync(cancellationToken);

        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Applicant Create Failed");
            throw;
        }
    }
}
