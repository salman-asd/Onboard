using ASD.Onboard.Application.Features.ApplicantProfiles.Applicants.Models;

namespace ASD.Onboard.Application.Features.ApplicantProfiles.Applicants.Queries;

public record GetApplicantQuery(Guid Id) : IRequest<ApplicantModel>;

internal sealed class GetApplicantQueryHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<GetApplicantQuery, ApplicantModel>
{

    public async Task<ApplicantModel> Handle(GetApplicantQuery request, CancellationToken cancellationToken)
    {
        var entity = await context.Applicants.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);

        Guard.Against.NotFound(request.Id, entity);

        var model = mapper.Map<ApplicantModel>(entity);

        return model;
    }
}
