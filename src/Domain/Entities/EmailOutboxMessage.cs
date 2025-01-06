using System.Text.Json;

namespace ASD.Onboard.Domain.Entities;

public class EmailOutboxMessage
{
    public Guid Id { get; private set; }
    public string To { get; private set; }
    public string Subject { get; private set; }
    public EmailType Type { get; private set; }
    public string? Body { get; private set; }
    public string? AttachmentPath { get; private set; }
    public string? TemplatePath { get; private set; }
    public string? SerializedTemplateModel { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public bool IsProcessed { get; private set; }
    public string? Error { get; private set; }
    public int RetryCount { get; private set; }

    private EmailOutboxMessage() { }

    public static EmailOutboxMessage CreatePlainEmail(string to, string subject, string body)
    {
        return new EmailOutboxMessage
        {
            Id = Guid.NewGuid(),
            To = to,
            Subject = subject,
            Body = body,
            Type = EmailType.Plain,
            CreatedAt = DateTime.UtcNow,
            IsProcessed = false,
            RetryCount = 0
        };
    }

    public static EmailOutboxMessage CreateHtmlEmail(string to, string subject, string htmlBody)
    {
        return new EmailOutboxMessage
        {
            Id = Guid.NewGuid(),
            To = to,
            Subject = subject,
            Body = htmlBody,
            Type = EmailType.Html,
            CreatedAt = DateTime.UtcNow,
            IsProcessed = false,
            RetryCount = 0
        };
    }

    public static EmailOutboxMessage CreateTemplateEmail(string to, string subject, string templatePath, object model)
    {
        return new EmailOutboxMessage
        {
            Id = Guid.NewGuid(),
            To = to,
            Subject = subject,
            TemplatePath = templatePath,
            SerializedTemplateModel = JsonSerializer.Serialize(model),
            Type = EmailType.Template,
            CreatedAt = DateTime.UtcNow,
            IsProcessed = false,
            RetryCount = 0
        };
    }

    public static EmailOutboxMessage CreateAttachmentEmail(string to, string subject, string body, string attachmentPath)
    {
        return new EmailOutboxMessage
        {
            Id = Guid.NewGuid(),
            To = to,
            Subject = subject,
            Body = body,
            AttachmentPath = attachmentPath,
            Type = EmailType.Attachment,
            CreatedAt = DateTime.UtcNow,
            IsProcessed = false,
            RetryCount = 0
        };
    }

    public void MarkAsProcessed()
    {
        ProcessedAt = DateTime.UtcNow;
        IsProcessed = true;
    }

    public void MarkAsFailed(string error)
    {
        Error = error;
        RetryCount++;
    }
}

public enum EmailType
{
    Plain,
    Html,
    Template,
    Attachment
}
