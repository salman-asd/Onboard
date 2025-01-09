namespace ASD.Onboard.Application.Features.PositionPosts.Models;

public record JobCardModel
{
    public Guid PostitionPostId { get; set; }
    public string? JobPostTitle { get; set; }
    public string? Reference { get; set; }
    public int Vacancy { get; set; }
    public decimal Experience { get; set; }
    public DateTime Deadline { get; set; }
    public int? StatusId { get; set; }
    public bool IsApplied { get; set; }
    public string? AppliedRef { get; set; }
}
