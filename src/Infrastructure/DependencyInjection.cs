using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Infrastructure.Data;
using ASD.Onboard.Infrastructure.Data.Interceptors;
using ASD.Onboard.Infrastructure.Extensions;
using ASD.Onboard.Infrastructure.Identity;
using ASD.Onboard.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddIdentity(configuration);

        services.AddAuthorizationBuilder();

        services.AddSingleton(TimeProvider.System);

        services.AddFluentEmail(configuration);

        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddHostedService<EmailHostedService>();
        services.AddSingleton<IEmailService, EmailService>();


        return services;
    }
}
