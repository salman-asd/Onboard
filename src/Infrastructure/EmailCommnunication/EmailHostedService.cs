using ASD.Onboard.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ASD.Onboard.Infrastructure.EmailCommnunication;

public class EmailHostedService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<EmailHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EmailHostedService(
        IBackgroundTaskQueue taskQueue,
        ILogger<EmailHostedService> logger,
        IServiceProvider serviceProvider)
    {
        _taskQueue = taskQueue;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email Hosted Service is starting.");

        await ProcessTaskQueueAsync(stoppingToken);
    }

    private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var workItem = await _taskQueue.DequeueAsync(stoppingToken);

                // Create a new scope for each dequeued task
                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        _logger.LogInformation("Processing email task");
                        await workItem(stoppingToken);
                        _logger.LogInformation("Email task processed successfully");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred while processing email task");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Normal shutdown, don't treat as error
                _logger.LogInformation("Email processing canceled.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while dequeuing task");

                // Add delay before next attempt to prevent tight loop in case of persistent errors
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email Hosted Service is stopping.");
        await base.StopAsync(stoppingToken);
    }
}
