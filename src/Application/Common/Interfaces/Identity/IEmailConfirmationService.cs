namespace ASD.Onboard.Application.Common.Interfaces.Identity;

public interface IEmailConfirmationService
{
    Task<Result> SendConfirmationEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(string email, Guid tokenId, CancellationToken cancellationToken = default);
}
