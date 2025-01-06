using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Domain.Entities;
using ASD.Onboard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ASD.Onboard.Infrastructure.EmailCommnunication;

internal class EmailOutboxRepository(ApplicationDbContext context) : IEmailOutboxRepository
{
    public async Task AddAsync(EmailOutboxMessage message, CancellationToken cancellationToken = default)
    {
        await context.EmailOutboxes.AddAsync(message, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<EmailOutboxMessage>> GetUnprocessedMessagesAsync(int batchSize = 10, CancellationToken cancellationToken = default)
    {
        return await context.EmailOutboxes
            .Where(x => !x.IsProcessed && x.RetryCount < 3)
            .OrderBy(x => x.CreatedAt)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(EmailOutboxMessage message, CancellationToken cancellationToken = default)
    {
        context.EmailOutboxes.Update(message);
        await context.SaveChangesAsync(cancellationToken);
    }
}
