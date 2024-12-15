namespace ASD.Onboard.Domain.Entities.Applicants;

public class ApplicantEducation: BaseAuditableEntity
{
    public Guid EducationLevel { get; set; }
    public string InstituteName { get; set; }
    public string CountryOfInstitute { get; set; }
    public string MajorSubject { get; set; }
    public int PassingYear { get; set; }
    public int ResultType { get; set; }
    public string Result { get; set; }
    public decimal ResultScale { get; set; }
    public Guid ApplicantId { get; set; }
    public string Board { get; set; }
    public bool IsHeighestEducation { get; set; }

    public virtual Applicant Applicant { get; set; } = default!;
}
