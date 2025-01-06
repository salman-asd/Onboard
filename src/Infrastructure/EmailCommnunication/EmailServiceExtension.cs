using System.Net;
using System.Net.Mail;
using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Infrastructure.EmailCommnunication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASD.Onboard.Infrastructure.EmailCommnunication;

internal static class EmailServiceExtension
{
    public static void AddFluentEmail(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
        Guard.Against.Null(emailSettings, nameof(emailSettings), "EmailSettings configuration section is missing.");

        // Validate essential email settings using Guard Clauses
        Guard.Against.NullOrEmpty(emailSettings.DefaultFromEmail, nameof(emailSettings.DefaultFromEmail));
        Guard.Against.NullOrEmpty(emailSettings.Host, nameof(emailSettings.Host));
        Guard.Against.NullOrEmpty(emailSettings.UserName, nameof(emailSettings.UserName));
        Guard.Against.NullOrEmpty(emailSettings.Password, nameof(emailSettings.Password));
        Guard.Against.NegativeOrZero(emailSettings.Port, nameof(emailSettings.Port));

        // Create NetworkCredential with domain if provided
        NetworkCredential credentials = new (emailSettings.UserName, emailSettings.Password);

        //var smtpClient = new SmtpClient
        //{
        //    Host = emailSettings.Host,
        //    Port = emailSettings.Port,
        //    EnableSsl = false,
        //    DeliveryMethod = SmtpDeliveryMethod.Network,
        //    UseDefaultCredentials = false,
        //    Credentials = credentials,
        //    TargetName = "STARTTLS/smtp"
        //};

        var smtpClient = new SmtpClient
        {
            Host = emailSettings.Host,
            Port = emailSettings.Port,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = credentials,
        };

        // Register FluentEmail with RazorRenderer and SMTP sender
        services
            .AddFluentEmail(emailSettings.DefaultFromEmail)
            .AddRazorRenderer()
            .AddSmtpSender(smtpClient);

        services.AddSingleton(emailSettings);
        //services.AddHostedService<EmailHostedService>();
        services.AddHostedService<EmailProcessingService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailOutboxRepository, EmailOutboxRepository>();
    }
}




