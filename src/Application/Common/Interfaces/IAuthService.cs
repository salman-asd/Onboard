using ASD.Onboard.Application.Common.Models;
using ASD.Onboard.Application.Features.Identity.Models;

namespace ASD.Onboard.Application.Common.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(string userName, string password);
    Task<Result> ForgotPasswordAsync(string email);
    Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    Task<Result> ResetPasswordAsync(string email, string token, string newPassword);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<bool> AuthorizeAsync(string userId, string policyName);
}
