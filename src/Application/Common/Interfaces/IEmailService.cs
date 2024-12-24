namespace ASD.Onboard.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmail(string to, string subject, string body);
    Task SendHtmlEmail(string to, string subject, string htmlBody);
    Task SendEmailWithAttachment(string to, string subject, string body, string attachmentPath);
    Task SendEmailWithRazorTemplate(string to, string subject, string templatePath, object model);
}
