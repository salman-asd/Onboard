using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Domain.Constants;
using ASD.Onboard.Infrastructure.Data;
using ASD.Onboard.Infrastructure.EmailCommnunication;
using ASD.Onboard.Infrastructure.Identity.Options;
using ASD.Onboard.Infrastructure.Identity.OptionSetup;
using ASD.Onboard.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASD.Onboard.Infrastructure.Identity;

internal static class IdentityServiceExtension
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<JwtOptions>();

        // Configure token providers first
        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(
                configuration.GetValue<int>("EmailConfirmationTokenExpiryHours", 24));
        });

        // Configure custom email confirmation token provider
        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;

            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.RequireUniqueEmail = true;
        });

        services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>("EmailConfirmation");

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
        services.AddTransient<ITokenEncrypDecryptService, TokenEncryptDecryptService>();
        services.AddScoped<ITokenStorageService, DatabaseTokenStorage>();
        services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
        return services;
    }
}
