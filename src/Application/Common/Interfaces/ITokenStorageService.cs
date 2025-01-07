namespace ASD.Onboard.Application.Common.Interfaces;

public interface ITokenStorageService
{
    Task<string> StoreTokenAsync(string userId, string token, TimeSpan lifetime, CancellationToken cancellationToken = default);
    Task<(bool isValid, string token)> ValidateTokenAsync(string userId, Guid tokenId);
    Task MarkTokenAsUsedAsync(Guid tokenId);
}
