using ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Models;

namespace ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Queries;

public record GetApplicantEducationQuery(Guid ApplicantId) : IRequest<List<ApplicantEducationModel>>;

internal sealed class GetApplicantEducationQueryHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<GetApplicantEducationQuery, List<ApplicantEducationModel>>
{

    public async Task<List<ApplicantEducationModel>> Handle(GetApplicantEducationQuery request, CancellationToken cancellationToken)
    {
        var educations = await context.ApplicantEducations
            .Where(x => x.ApplicantId == request.ApplicantId)
            .ProjectTo<ApplicantEducationModel>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return educations;
    }
}
