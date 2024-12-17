namespace ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Models;

public record ApplicantEducationModel
{
    public Guid Id { get; set; }
    public Guid? EducationLevel { get; set; }
    public string? InstituteName { get; set; }
    public string? CountryOfInstitute { get; set; }
    public string? MajorSubject { get; set; }
    public int? PassingYear { get; set; }
    public int? ResultType { get; set; }
    public string? Result { get; set; }
    public decimal? ResultScale { get; set; }
    public Guid ApplicantId { get; set; }
    public string? Board { get; set; }
    public bool IsHeighestEducation { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ApplicantEducation, ApplicantEducationModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicantId, opt => opt.Ignore());

            CreateMap<ApplicantEducationModel, ApplicantEducation>();
        }
    }
}
