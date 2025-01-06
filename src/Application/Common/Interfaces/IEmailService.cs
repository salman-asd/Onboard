namespace ASD.Onboard.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    Task SendHtmlEmailAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default);
    Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentPath, CancellationToken cancellationToken = default);
    Task SendEmailWithRazorTemplateAsync(string to, string subject, string templatePath, object model, CancellationToken cancellationToken = default);
}
