namespace ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Commands;

public record CreateApplicantEducationCommand(
    Guid? EducationLevel,
    string? InstituteName,
    string? CountryOfInstitute,
    string? MajorSubject,
    int? PassingYear,
    int? ResultType,
    string? Result,
    decimal? ResultScale,
    Guid ApplicantId,
    string? Board,
    bool IsHeighestEducation,
    Applicant Applicant
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

