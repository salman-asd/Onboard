namespace ASD.Onboard.Domain.Entities.Applicants;

public class Applicant : BaseAuditableEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PreferredName { get; set; }
    public string UserId { get; set; }
    public DateOnly? DOB { get; set; }
    public int? BloodGroupId { get; set; }
    public Guid? ReligionId { get; set; }
    public int? MaritalStatusId { get; set; }
    public int? GenderId { get; set; }
    public string? PrimaryMobileNo { get; set; }
    public string? SecondaryMobileNo { get; set; }
    public string? PrimaryEmail { get; set; }
    public string? SecondaryEmail { get; set; }
    public string? Nationality { get; set; }
    public int? IdentificationType { get; set; }
    public long? IdentificationNo { get; set; }
    public string? PermAddress { get; set; }
    public Guid? PermDistrictId { get; set; }
    public int? PermZipCode { get; set; }
    public string? PresAddress { get; set; }
    public Guid? PresDistrictId { get; set; }
    public int? PresZipCode { get; set; }
    public int? ContactAddress { get; set; }

    public virtual List<ApplicantEducation> ApplicantEducations { get; set; } = [];

    public static Applicant Create(
        string userId,
        string firstName, 
        string lastName, 
        string email)
    {
        return new Applicant
        {
            UserId = userId,
            FirstName = firstName,
            LastName = lastName,
            PreferredName = firstName,
            PrimaryEmail = email,
            Created = DateTime.Now,
            CreatedBy = userId
        };
    }
}


