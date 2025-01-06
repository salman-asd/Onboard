using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ASD.Onboard.Infrastructure.EmailCommnunication;

internal sealed class EmailProcessingService(
    IServiceProvider serviceProvider,
    ILogger<EmailProcessingService> logger,
    IConfiguration configuration) : BackgroundService
{
    private readonly int _delayMilliseconds = configuration.GetValue<int>("EmailProcessing:DelayMilliseconds", 10000);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessEmailsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing emails");
            }

            await Task.Delay(_delayMilliseconds, stoppingToken);
        }
    }

    private async Task ProcessEmailsAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var emailOutboxRepository = scope.ServiceProvider.GetRequiredService<IEmailOutboxRepository>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var messages = await emailOutboxRepository.GetUnprocessedMessagesAsync(cancellationToken: stoppingToken);

        foreach (var message in messages)
        {
            try
            {
                switch (message.Type)
                {
                    case EmailType.Plain:
                        await emailService.SendEmailAsync(
                            message.To,
                            message.Subject,
                            message.Body,
                            stoppingToken);
                        break;

                    case EmailType.Html:
                        await emailService.SendHtmlEmailAsync(
                            message.To,
                            message.Subject,
                            message.Body,
                            stoppingToken);
                        break;

                    case EmailType.Template:
                        var model = JsonSerializer.Deserialize<object>(message.SerializedTemplateModel);
                        await emailService.SendEmailWithRazorTemplateAsync(
                            message.To,
                            message.Subject,
                            message.TemplatePath,
                            model,
                            stoppingToken);
                        break;

                    case EmailType.Attachment:
                        await emailService.SendEmailWithAttachmentAsync(
                            message.To,
                            message.Subject,
                            message.Body,
                            message.AttachmentPath,
                            stoppingToken);
                        break;
                }

                message.MarkAsProcessed();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing email message {MessageId}", message.Id);
                message.MarkAsFailed(ex.Message);
            }

            await emailOutboxRepository.UpdateAsync(message, stoppingToken);
        }
    }
}
