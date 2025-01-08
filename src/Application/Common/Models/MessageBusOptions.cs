namespace ASD.Onboard.Application.Common.Models;

public class MessageBusOptions
{
    public string Host { get; set; }
    public string VirtualHost { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Exchange { get; set; }
    public string Version { get; set; }
    public string TopicPrefix { get; set; }
    public int FailedRetryCount { get; set; }
    public int FailedRetryInterval { get; set; }
}
