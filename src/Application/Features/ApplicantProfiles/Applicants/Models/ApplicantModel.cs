namespace ASD.Onboard.Application.Features.ApplicantProfiles.Applicants.Models;

public record ApplicantModel
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PreferredName { get; set; }
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

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Applicant, ApplicantModel>();
            CreateMap<ApplicantModel, Applicant>();
        }
    }
}


