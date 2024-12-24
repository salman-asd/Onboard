using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASD.Onboard.Infrastructure.Extensions;

internal static class EmailServiceExtension
{
    public static void AddFluentEmail(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection("EmailSettings");
        var defaultFromEmail = emailSettings["DefaultFromEmail"];
        var host = emailSettings["SMTPSetting:Host"];
        var port = emailSettings.GetValue<int>("SMTPSetting:Port");
        var userName = emailSettings["SMTPSetting:UserName"];
        var password = emailSettings["SMTPSetting:Password"];

        services
            .AddFluentEmail(defaultFromEmail)
            .AddRazorRenderer()
            .AddSmtpSender(host, port, userName, password);
    }

}


