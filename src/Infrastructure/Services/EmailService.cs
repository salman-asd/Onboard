using ASD.Onboard.Application.Common.Interfaces;
using FluentEmail.Core;

namespace ASD.Onboard.Infrastructure.Services;

internal sealed class EmailService(IFluentEmail fluentEmail) : IEmailService
{
    public async Task SendEmail(string to, string subject, string body)
    {
        await fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body)
            .SendAsync();
    }

    public async Task SendHtmlEmail(string to, string subject, string htmlBody)
    {
        await fluentEmail
            .To(to)
            .Subject(subject)
            .Body(htmlBody, true)
            .SendAsync();
    }

    public async Task SendEmailWithAttachment(string to, string subject, string body, string attachmentPath)
    {
        await fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body)
            .AttachFromFilename(attachmentPath)
            .SendAsync();
    }
   // await emailService.SendEmailWithRazorTemplate("recipient@example.com", "Subject", "wwwroot/templates/emailTemplate.cshtml", model);

    public async Task SendEmailWithRazorTemplate(string to, string subject, string templatePath, object model)
    {
        await fluentEmail
            .To(to)
            .Subject(subject)
            .UsingTemplate(templatePath, model)
            .SendAsync();
    }
}


