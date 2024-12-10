namespace ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Models;

public record ApplicantEducationModel
{
    public Guid Id { get; set; }
    public int EducationLevel { get; set; }
    public string InstituteName { get; set; }
    public int CountryOfInstitute { get; set; }
    public string MajorSubject { get; set; }
    public int PassingYear { get; set; }
    public string Result { get; set; }
    public Guid ApplicantId { get; set; }

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
