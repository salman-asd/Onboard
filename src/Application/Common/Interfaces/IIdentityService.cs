using ASD.Onboard.Application.Common.Models;
using ASD.Onboard.Application.Features.Identity.Commands;

namespace ASD.Onboard.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<Result> CreateUserAsync(UserRegisterCommand request);

    Task<Result> DeleteUserAsync(string userId);
}
