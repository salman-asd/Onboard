using ASD.Onboard.Application.Common.Interfaces.Identity;
using ASD.Onboard.Application.Common.Models;
using ASD.Onboard.Application.Features.Identity.Commands;
using Microsoft.AspNetCore.Identity;

namespace ASD.Onboard.Infrastructure.Identity;

public class IdentityService(
    UserManager<AppUser> userManager,
    IEmailConfirmationService emailConfirmationService) : IIdentityService
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
            await SendConfirmationEmailAsync(request.Email);
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

    public async Task<Result> SendConfirmationEmailAsync(string email)
    {
        return await emailConfirmationService.SendConfirmationEmailAsync(email);
    }
}
