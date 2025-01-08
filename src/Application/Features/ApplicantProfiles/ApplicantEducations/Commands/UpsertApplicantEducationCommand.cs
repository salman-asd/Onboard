using ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Models;

namespace ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Commands;

public record UpsertApplicantEducationCommand(
    List<ApplicantEducationModel> ApplicantEducations
    ) : IRequest
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpsertApplicantEducationCommand, ApplicantEducation>();
        }
    }
}

internal sealed class UpsertApplicantEducationCommandHandler(
    IApplicationDbContext context,
    IApplicantService applicantService,
    IMapper mapper)
    : IRequestHandler<UpsertApplicantEducationCommand>
{

    public async Task Handle(UpsertApplicantEducationCommand request, CancellationToken cancellationToken)
    {
        var applicantId = await applicantService.GetApplicantIdAsync(cancellationToken);
        Guard.Against.NullOrEmpty(applicantId);

        var existedEducations =  context.ApplicantEducations
            .Where(x => x.ApplicantId == applicantId);

        context.ApplicantEducations.RemoveRange(existedEducations);

        var newEducations = mapper.Map<List<ApplicantEducation>>(request.ApplicantEducations);
        newEducations.ForEach(x => x.ApplicantId = applicantId.Value);

        context.ApplicantEducations.AddRange(newEducations);

        await context.SaveChangesAsync(cancellationToken);
    }
}
