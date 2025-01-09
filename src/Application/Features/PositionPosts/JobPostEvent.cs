namespace ASD.Onboard.Application.Features.PositionPosts;

public class JobPostEvent
{
    public Guid JobPostId { get; set; }
    public string PostName { get; set; } = string.Empty;
}
