namespace ASD.Onboard.Application.Features.JobPosts;

public class JobPostEvent
{
    public Guid JobPostId { get; set; }
    public string PostName { get; set; } = string.Empty;
}
