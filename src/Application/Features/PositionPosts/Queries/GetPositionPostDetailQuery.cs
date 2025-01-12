using ASD.Onboard.Application.Common.Security;
using ASD.Onboard.Application.Features.PositionPosts.Models;

namespace ASD.Onboard.Application.Features.PositionPosts.Queries;

[Authorize]
public record GetPositionPostDetailQuery(
    Guid PositionPostId) : IRequest<PositionPostModel>;

internal sealed class GetPositionPostDetailQueryHandler(
    IApplicationDbContext context, 
    IMapper mapper) 
    : IRequestHandler<GetPositionPostDetailQuery, PositionPostModel>
{
    public async Task<PositionPostModel> Handle(GetPositionPostDetailQuery request, CancellationToken cancellationToken)
    {
        var positionPost = await context.PositionPosts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.PositionPostId, cancellationToken);

        return mapper.Map<PositionPostModel>(positionPost);
    }
}
