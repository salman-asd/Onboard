using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Application.Common.Models;
using ASD.Onboard.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASD.Onboard.Infrastructure.Cap;

internal static class MessageBusExtensions
{
    public static IServiceCollection AddMessageBus(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = new MessageBusOptions();
        configuration.GetSection("MessageBus").Bind(options);

        services.AddCap(x =>
        {
            // RabbitMQ Configuration
            x.UseRabbitMQ(o =>
            {
                o.HostName = options.Host;
                o.VirtualHost = options.VirtualHost;
                o.UserName = options.Username;
                o.Password = options.Password;
                //o.ExchangeName = options.Exchange;
            });

            // Database Configuration for Storage
            x.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            x.UseEntityFramework<ApplicationDbContext>();
            // General CAP Configuration
            //x.Version = options.Version;
            //x.TopicNamePrefix = options.TopicPrefix;
            x.FailedRetryCount = options.FailedRetryCount;
            x.FailedRetryInterval = options.FailedRetryInterval;

            // Optional: Dashboard for monitoring
            x.UseDashboard();
        });

        services.AddScoped<IMessageBus, CapMessageBus>();
        services.AddScoped<JobPostSubscriber>();

        return services;
    }
}
