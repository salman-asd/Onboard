using ASD.Onboard.Application.Common.Interfaces.Identity;
using ASD.Onboard.Application.Common.Security;
using ASD.Onboard.Application.Features.ApplicantProfiles.Applicants.Models;

namespace ASD.Onboard.Application.Features.ApplicantProfiles.Applicants.Queries;

[Authorize]
public record GetApplicantQuery: IRequest<ApplicantModel>;

internal sealed class GetApplicantQueryHandler(
    IApplicationDbContext context,
    IMapper mapper,
    IUser user)
    : IRequestHandler<GetApplicantQuery, ApplicantModel>
{
    public async Task<ApplicantModel> Handle(GetApplicantQuery request, CancellationToken cancellationToken)
    {
        var entity = await context.Applicants
            .SingleOrDefaultAsync(x => x.UserId.ToLower() == user.Id, cancellationToken)
            .ConfigureAwait(false);

        var model = mapper.Map<ApplicantModel>(entity);

        return model;
    }
}
