using ASD.Onboard.Application.Common.Exceptions;
using ASD.Onboard.Domain.Entities.Jobs;
using FluentValidation.Results;
using ValidationException = ASD.Onboard.Application.Common.Exceptions.ValidationException;

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

            if (await context.JobApplications.AnyAsync(x => x.ApplicantId == applicantId
                 && x.PositionPostId == request.PositionPostId))
            {
                throw new AlreadyAppliedException("You have already applied.");
            }

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

