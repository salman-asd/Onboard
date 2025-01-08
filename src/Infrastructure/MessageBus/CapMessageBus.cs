using ASD.Onboard.Application.Common.Interfaces;
using DotNetCore.CAP;

namespace ASD.Onboard.Infrastructure.Cap;

public class CapMessageBus(ICapPublisher capPublisher) : IMessageBus
{
    public async Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class
    {
        await capPublisher.PublishAsync(topic, message, cancellationToken: cancellationToken);
    }

    public async Task PublishAsync<T>(string topic, T message, IDictionary<string, string> headers, CancellationToken cancellationToken = default) where T : class
    {
        await capPublisher.PublishAsync(topic, message, headers, cancellationToken);
    }
}
