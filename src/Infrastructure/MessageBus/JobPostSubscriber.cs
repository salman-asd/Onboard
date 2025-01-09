using ASD.Onboard.Application.Features.PositionPosts;
using DotNetCore.CAP;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASD.Onboard.Infrastructure.Cap;

internal sealed class JobPostSubscriber(ILogger<JobPostSubscriber> logger, ISender sender) : ICapSubscribe
{
    [CapSubscribe("jobpost")]
    public async Task HandleJobPostsync(JobPostEvent @event, [FromCap] CapHeader header)
    {
        try
        {
            logger.LogInformation($"Received order created event for Order {@event.JobPostId}");
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
