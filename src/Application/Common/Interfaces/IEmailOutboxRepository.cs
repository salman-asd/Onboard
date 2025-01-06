using ASD.Onboard.Domain.Entities;

namespace ASD.Onboard.Application.Common.Interfaces;

public interface IEmailOutboxRepository
{
    Task AddAsync(EmailOutboxMessage message, CancellationToken cancellationToken = default);
    Task<IEnumerable<EmailOutboxMessage>> GetUnprocessedMessagesAsync(int batchSize = 10, CancellationToken cancellationToken = default);
    Task UpdateAsync(EmailOutboxMessage message, CancellationToken cancellationToken = default);
}
