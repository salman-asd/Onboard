namespace ASD.Onboard.Application.Features.ApplicantProfiles.Applicants.Commands;

public record UpdateApplicantCommand(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? PreferredName,
    DateOnly? DOB,
    int? BloodGroupId,
    Guid? ReligionId,
    int? MaritalStatusId,
    int? GenderId,
    string? PrimaryMobileNo,
    string? SecondaryMobileNo,
    string? PrimaryEmail,
    string? SecondaryEmail,
    string? Nationality,
    int? IdentificationType,
    long? IdentificationNo,
    string? PermAddress,
    Guid? PermDistrictId,
    int? PermZipCode,
    string? PresAddress,
    Guid? PresDistrictId,
    int? PresZipCode,
    int ContactAddress) : IRequest
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateApplicantCommand, Applicant>();
        }
    }
}

internal sealed class UpdateApplicantCommandCommandHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<UpdateApplicantCommand>
{

    public async Task Handle(UpdateApplicantCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Applicants.FindAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity = mapper.Map(request, entity);

        context.Applicants.Update(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}
