
using ASD.Onboard.Domain.Entities.Jobs;

namespace ASD.Onboard.Application.Features.PositionPosts.Commands;

public record CreateJobApplicationCommand(
    Guid PositionPostId) : IRequest;

internal sealed class CreateJobApplicationCommandHandler(
    IApplicationDbContext context, 
    IApplicantService applicantService) : IRequestHandler<CreateJobApplicationCommand>
{
    public async Task Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var applicantId = await applicantService.GetApplicantIdAsync(cancellationToken);

            Guard.Against.NullOrEmpty(applicantId, nameof(applicantId));

            var jobApplication = new JobApplication
            {
                ApplicantId = applicantId.Value,
                PositionPostId = request.PositionPostId,
                AppliedRef = Guid.NewGuid().ToString(),
            };

            context.JobApplications.Add(jobApplication);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}

