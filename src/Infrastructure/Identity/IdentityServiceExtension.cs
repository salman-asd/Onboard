using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Domain.Constants;
using ASD.Onboard.Infrastructure.Data;
using ASD.Onboard.Infrastructure.Identity.Options;
using ASD.Onboard.Infrastructure.Identity.OptionSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASD.Onboard.Infrastructure.Identity;

internal static class IdentityServiceExtension
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<JwtOptions>();

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        //services.AddIdentity<AppUser, IdentityRole>()
        //    .AddEntityFrameworkStores<ApplicationDbContext>()
        //    .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.ConfigureOptions<ConfigureJwtBearerOptions>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator));
        });

        services.AddTransient<ITokenProvider, TokenProvider>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IIdentityService, IdentityService>();

        return services;
    }
}
