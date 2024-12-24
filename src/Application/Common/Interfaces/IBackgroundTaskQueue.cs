namespace ASD.Onboard.Application.Common.Interfaces;

public interface IBackgroundTaskQueue
{
    void Enqueue(Func<CancellationToken, Task> task);
    Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    Task EnqueueWithRetry(Func<CancellationToken, Task> task, int maxRetryCount);
}



