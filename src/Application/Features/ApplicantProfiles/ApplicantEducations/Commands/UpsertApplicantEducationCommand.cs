using ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Models;

namespace ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Commands;

public record UpsertApplicantEducationCommand(
    Guid ApplicantId,
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
    IMapper mapper)
    : IRequestHandler<UpsertApplicantEducationCommand>
{

    public async Task Handle(UpsertApplicantEducationCommand request, CancellationToken cancellationToken)
    {
        var existedEducations =  context.ApplicantEducations
            .Where(x => x.ApplicantId == request.ApplicantId);

        context.ApplicantEducations.RemoveRange(existedEducations);

        var newEducations = mapper.Map<List<ApplicantEducation>>(request.ApplicantEducations);
        newEducations.ForEach(x => x.ApplicantId = request.ApplicantId);

        context.ApplicantEducations.AddRange(newEducations);

        await context.SaveChangesAsync(cancellationToken);
    }
}
