using System.Linq.Expressions;

namespace ASD.Onboard.Application.Common.Interfaces;

public interface IBackgroundJobService
{
    /// <summary>
    /// Enqueue a background job to run immediately.
    /// </summary>
    void Enqueue(Action job);

    /// <summary>
    /// Enqueue a background job with arguments.
    /// </summary>
    void Enqueue<T>(Expression<Func<T, Task>> job);

    /// <summary>
    /// Schedule a job to run at a specific time.
    /// </summary>
    void Schedule<T>(Expression<Func<T, Task>> job, TimeSpan delay);

    /// <summary>
    /// Add a recurring job that runs periodically.
    /// </summary>
    void AddRecurringJob<T>(string jobId, Expression<Func<T, Task>> job, string cronExpression);

    /// <summary>
    /// Remove a recurring job.
    /// </summary>
    void RemoveRecurringJob(string jobId);

    /// <summary>
    /// Continue a job after another job has completed.
    /// </summary>
    void ContinueWith(string parentJobId, Action job);
}

