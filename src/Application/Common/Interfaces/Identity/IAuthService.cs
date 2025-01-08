using ASD.Onboard.Application.Common.Models;
using ASD.Onboard.Application.Features.Identity.Models;

namespace ASD.Onboard.Application.Common.Interfaces.Identity;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(string userName, string password);
    Task<Result> ForgotPasswordAsync(string email);
    Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    Task<Result> ResetPasswordAsync(string email, string token, string newPassword);
    Task<Result> ConfirmEmailAsync(string email, Guid tokenId, CancellationToken cancellationToken = default);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<bool> AuthorizeAsync(string userId, string policyName);
}
