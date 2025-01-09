namespace ASD.Onboard.Domain.Entities.Jobs;

public class PositionPost: BaseAuditableEntity
{
    public string? RefNo { get; set; }
    public Guid? PositionBasedReqId { get; set; }
    public string? JobPostTitle { get; set; }
    public Guid? CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public Guid? DesignationId { get; set; }
    public string? DesignationName { get; set; }
    public int? PeopleRequired { get; set; }
    public decimal? Experience { get; set; }
    public DateTime? ValidUpTo { get; set; }
    public Guid? EmploymentStatusId { get; set; }
    public string? EmploymentStatusName { get; set; }
    public string? SalaryRange { get; set; }
    public string? Location { get; set; }
    public bool? ShowSalary { get; set; }
    public string? Discover { get; set; }
    public string? RoleOverview { get; set; }
    public string? EducationExperience { get; set; }
    public string? OtherSkills { get; set; }
    public string? JobLocation { get; set; }
    public string? Benefits { get; set; }
    public string? Others { get; set; }
    public int? StatusId { get; set; }
}
