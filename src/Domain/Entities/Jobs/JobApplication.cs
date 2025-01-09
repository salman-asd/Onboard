namespace ASD.Onboard.Domain.Entities.Jobs;

public class JobApplication: BaseAuditableEntity
{
    public Guid ApplicantId { get; set; }
    public Guid PositionPostId { get; set; }
    public ApplicationStatus Status { get; set; }
    public string? AppliedRef { get; set; }
}

public enum ApplicationStatus
{
    Applied,
    Shortlisted,
    Rejected,
    Hired
}
