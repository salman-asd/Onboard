namespace ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Commands;

public record CreateApplicantEducationCommand(
    int EducationLevel,
    string InstituteName,
    int CountryOfInstitute,
    string MajorSubject,
    int PassingYear,
    string Result,
    Guid ApplicantId
    ) : IRequest<Guid>
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateApplicantEducationCommand, ApplicantEducation>();
        }
    }
}

internal sealed class CreateApplicantEducationCommandHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<CreateApplicantEducationCommand, Guid>
{

    public async Task<Guid> Handle(CreateApplicantEducationCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<ApplicantEducation>(request);

        context.ApplicantEducations.AddRange(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
