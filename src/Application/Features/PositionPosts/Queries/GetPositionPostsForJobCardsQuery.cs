using ASD.Onboard.Application.Common.Security;
using ASD.Onboard.Application.Features.PositionPosts.Models;
using Microsoft.EntityFrameworkCore;

namespace ASD.Onboard.Application.Features.PositionPosts.Queries;

[Authorize]
public record GetPositionPostsForJobCardsQuery: IRequest<List<JobCardModel>>;

internal sealed class GetPositionPostsForApplyQueryHandler(
    IApplicationDbContext context, 
    IMapper mapper,
    IApplicantService applicantService) : IRequestHandler<GetPositionPostsForJobCardsQuery, List<JobCardModel>>
{
    public async Task<List<JobCardModel>> Handle(GetPositionPostsForJobCardsQuery request, CancellationToken cancellationToken)
    {
        var applicantId = await applicantService.GetApplicantIdAsync(cancellationToken);

        return await context.PositionPosts
            .Select(x => new JobCardModel
            {
                PostitionPostId = x.Id,
                JobPostTitle = x.JobPostTitle,
                Reference = x.RefNo,
                Vacancy = x.PeopleRequired ?? 0,
                Experience = x.Experience ?? 0,
                Deadline = x.ValidUpTo.Value,
                StatusId = x.StatusId,
                IsApplied = context.JobApplications
                    .Where(ja => ja.ApplicantId == applicantId && ja.PositionPostId == x.Id)
                    .Select(ja => true)
                    .FirstOrDefault(),
                AppliedRef = context.JobApplications
                    .Where(ja => ja.ApplicantId == applicantId && ja.PositionPostId == x.Id)
                    .Select(ja => ja.AppliedRef)
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);
    }
}
