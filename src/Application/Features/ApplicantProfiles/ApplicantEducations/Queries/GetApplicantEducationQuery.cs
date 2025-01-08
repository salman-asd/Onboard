using ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Models;

namespace ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Queries;

public record GetApplicantEducationQuery : IRequest<List<ApplicantEducationModel>>;

internal sealed class GetApplicantEducationQueryHandler(
    IApplicationDbContext context,
    IApplicantService applicantService,
    IMapper mapper)
    : IRequestHandler<GetApplicantEducationQuery, List<ApplicantEducationModel>>
{

    public async Task<List<ApplicantEducationModel>> Handle(GetApplicantEducationQuery request, CancellationToken cancellationToken)
    {
        var applicantId = await applicantService.GetApplicantIdAsync(cancellationToken);

        Guard.Against.NullOrEmpty(applicantId);

        var educations = await context.ApplicantEducations
            .Where(x => x.ApplicantId == applicantId)
            .ProjectTo<ApplicantEducationModel>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if(educations.Count == 0)
        {
            educations.Add(new ApplicantEducationModel
            {
                ApplicantId = applicantId.Value,

            });
        }

        return educations;
    }
}
