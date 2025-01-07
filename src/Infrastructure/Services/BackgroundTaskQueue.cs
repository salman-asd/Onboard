using System.Threading.Channels;
using ASD.Onboard.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace ASD.Onboard.Infrastructure.Services;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, Task>> _queue;
    private readonly ILogger<BackgroundTaskQueue> _logger;

    public BackgroundTaskQueue(ILogger<BackgroundTaskQueue> logger)
    {
        // Create bounded channel with options for better back-pressure handling
        var options = new BoundedChannelOptions(capacity: 100)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<Func<CancellationToken, Task>>(options);
        _logger = logger;
    }

    public async Task Enqueue(Func<CancellationToken, Task> task)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));

        // Use WriteAsync instead of TryWrite to ensure the task is queued
        await _queue.Writer.WriteAsync(task);
        _logger.LogInformation("Task successfully enqueued");
    }

    public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
    {
        var workItem = await _queue.Reader.ReadAsync(cancellationToken);
        _logger.LogInformation("Task dequeued for processing");
        return workItem;
    }

    public async Task EnqueueWithRetry(Func<CancellationToken, Task> task, int maxRetryCount)
    {
        int attempt = 0;
        var delay = TimeSpan.FromSeconds(1);

        while (attempt < maxRetryCount)
        {
            try
            {
                await Enqueue(task);
                _logger.LogInformation("Task enqueued successfully after {Attempt} attempts", attempt + 1);
                return;
            }
            catch (Exception ex)
            {
                attempt++;
                _logger.LogWarning(ex, "Failed to enqueue task. Attempt {Attempt} of {MaxRetries}",
                    attempt, maxRetryCount);

                if (attempt == maxRetryCount)
                {
                    _logger.LogError(ex, "Task failed to enqueue after {MaxRetries} attempts", maxRetryCount);
                    throw new Exception($"Failed to enqueue task after {maxRetryCount} attempts.", ex);
                }

                // Exponential backoff
                await Task.Delay(delay);
                delay *= 2;
            }
        }
    }
}
