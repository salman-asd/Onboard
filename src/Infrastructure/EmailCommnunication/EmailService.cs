using ASD.Onboard.Application.Common.Interfaces;
using FluentEmail.Core;

namespace ASD.Onboard.Infrastructure.EmailCommnunication;

internal sealed class EmailService(
    IFluentEmail fluentEmail) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var email = fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body);

        var response = await email.SendAsync(cancellationToken);
        if (!response.Successful)
        {
            throw new Exception($"Failed to send email: {string.Join(", ", response.ErrorMessages)}");
        }
    }

    public async Task SendHtmlEmailAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        var email = fluentEmail
            .To(to)
            .Subject(subject)
            .Body(htmlBody, true);

        var response = await email.SendAsync(cancellationToken);
        if (!response.Successful)
        {
            throw new Exception($"Failed to send email: {string.Join(", ", response.ErrorMessages)}");
        }
    }

    public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentPath, CancellationToken cancellationToken = default)
    {
        var email = fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body)
            .AttachFromFilename(attachmentPath);

        var response = await email.SendAsync(cancellationToken);
        if (!response.Successful)
        {
            throw new Exception($"Failed to send email: {string.Join(", ", response.ErrorMessages)}");
        }
    }

    public async Task SendEmailWithRazorTemplateAsync(string to, string subject, string templatePath, object model, CancellationToken cancellationToken = default)
    {
        var email = fluentEmail
            .To(to)
            .Subject(subject)
            //.UsingTemplate(templatePath, model);
            .UsingTemplateFromFile(templatePath, model);

        var response = await email.SendAsync(cancellationToken);
        if (!response.Successful)
        {
            throw new Exception($"Failed to send email: {string.Join(", ", response.ErrorMessages)}");
        }
    }
}

