using ASD.Onboard.Application.Features.PositionPosts.Models;

namespace ASD.Onboard.Application.Features.PositionPosts.Queries;

public record GetPositionPostsForJobCardsQuery: IRequest<List<JobCardModel>>;

internal sealed class GetPositionPostsForApplyQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetPositionPostsForJobCardsQuery, List<JobCardModel>>
{
    public async Task<List<JobCardModel>> Handle(GetPositionPostsForJobCardsQuery request, CancellationToken cancellationToken)
    {
        var positionPosts = await context.PositionPosts
            .Select(x => new JobCardModel
            {
                PostitionPostId = x.Id,
                JobPostTitle = x.JobPostTitle,
                Reference = x.RefNo,
                Vacancy = x.PeopleRequired ?? 0,
                Experience = x.Experience ?? 0,
                Deadline = x.ValidUpTo.Value,
                StatusId = x.StatusId
                //IsApplied = context.JobApplications.Any(ja => ja.ApplicantId == 1),
                //AppliedRef = x.JobApplications.FirstOrDefault(ja => ja.ApplicantId == 1)?.Reference
            })
            .ToListAsync(cancellationToken);

        return mapper.Map<List<JobCardModel>>(positionPosts);
    }
}
