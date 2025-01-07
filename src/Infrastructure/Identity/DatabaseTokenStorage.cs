using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ASD.Onboard.Infrastructure.Identity;

public class DatabaseTokenStorage(ApplicationDbContext context, ITokenEncrypDecryptService encryptionService) : ITokenStorageService
{
    public async Task<string> StoreTokenAsync(string userId, string token, TimeSpan lifetime, CancellationToken cancellationToken = default)
    {
        var encryptedToken = encryptionService.EncryptToken(token);
        var tokenEntity = EmailConfirmationToken.Create(userId, lifetime);

        context.EmailConfirmationTokens.Add(tokenEntity);
        await context.SaveChangesAsync();

        return tokenEntity.Id.ToString();
    }

    public async Task<(bool isValid, string token)> ValidateTokenAsync(string userId, Guid tokenId)
    {
        var tokenEntity = await context.EmailConfirmationTokens
            .FirstOrDefaultAsync(t => t.Id == tokenId && t.UserId == userId);

        if (tokenEntity == null || tokenEntity.IsUsed || tokenEntity.ExpiryTime < DateTime.Now)
            return (false, null);

        return (true, encryptionService.DecryptToken(tokenEntity.Id.ToString()));
    }

    public async Task MarkTokenAsUsedAsync(Guid tokenId)
    {
        var token = await context.EmailConfirmationTokens.FindAsync(tokenId);
        if (token != null)
        {
            token.IsUsed = true;
            await context.SaveChangesAsync();
        }
    }
}
