namespace ASD.Onboard.Application.Common.Interfaces;

public interface IMessageBus
{
    Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class;
    Task PublishAsync<T>(string topic, T message, IDictionary<string, string> headers, CancellationToken cancellationToken = default) where T : class;
}
