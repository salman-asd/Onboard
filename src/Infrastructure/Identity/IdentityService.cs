using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Application.Common.Models;
using ASD.Onboard.Application.Features.Identity.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASD.Onboard.Infrastructure.Identity;

public class IdentityService(
    UserManager<AppUser> userManager) : IIdentityService
{
    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        return user?.UserName;
    }

    public async Task<Result> CreateUserAsync(UserRegisterCommand request)
    {
        var user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNo,
            UserName = request.Email,
            Email = request.Email
        };
        var result = await userManager.CreateAsync(user, request.Password);

        return result.ToApplicationResult();
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(AppUser user)
    {
        var result = await userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

}
