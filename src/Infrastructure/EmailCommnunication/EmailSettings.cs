namespace ASD.Onboard.Infrastructure.EmailCommnunication;

public sealed class EmailSettings
{
    public const string EMAIL_SETTINGS = "EmailSettings";
    public string DefaultFromEmail { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int OutboxRetry { get; set; } = 10;
}
