namespace ASD.Onboard.Domain.Entities.Applicants;

public class ApplicantEducation: BaseAuditableEntity
{
    public int EducationLevel { get; set; }
    public string InstituteName { get; set; }
    public string CountryOfInstitute { get; set; }
    public string MajorSubject { get; set; }
    public int PassingYear { get; set; }
    public string Result { get; set; }
    public Guid ApplicantId { get; set; }

    public virtual Applicant Applicant { get; set; } = default!;
}
