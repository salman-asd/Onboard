using System.Threading.Channels;
using ASD.Onboard.Application.Common.Interfaces;

namespace ASD.Onboard.Infrastructure.Services;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, Task>> _queue;

    public BackgroundTaskQueue()
    {
        _queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>();
    }

    public void Enqueue(Func<CancellationToken, Task> task)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));
        _queue.Writer.TryWrite(task);
    }

    public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }

    public async Task EnqueueWithRetry(Func<CancellationToken, Task> task, int maxRetryCount)
    {
        int attempt = 0;

        while (attempt < maxRetryCount)
        {
            try
            {
                Enqueue(task); // Directly enqueue the task
                break; // Break the loop if the task is enqueued successfully
            }
            catch (Exception ex)
            {
                if (attempt == maxRetryCount - 1)
                {
                    throw new Exception($"Task failed after {maxRetryCount} attempts.", ex);
                }

                attempt++;
                await Task.Delay(1000); // Optional: add delay before retrying
            }
        }
    }
}
