﻿namespace ASD.Onboard.Application.Features.ApplicantProfiles.Applicants.Commands;

public record CreateApplicantCommand(
    string FirstName,
    string LastName,
    string PreferredName,
    DateOnly DOB,
    int BloodGroupId,
    Guid ReligionId,
    int MaritalStatusId,
    int GenderId,
    string PrimaryMobileNo,
    string? SecondaryMobileNo,
    string PrimaryEmail,
    string? SecondaryEmail,
    int NationalityId,
    int IdentificationType,
    long IdentificationNo,
    string PermAddress,
    Guid PermDistrictId,
    int PermZipCode,
    string PresAddress,
    Guid? PresDistrictId,
    int? PresZipCode,
    int ContactAddress
    ) : IRequest<Guid>
{
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateApplicantCommand, Applicant>();
        }
    }
}

internal sealed class CreateApplicantCommandHandler(
    IApplicationDbContext context,
    IMapper mapper)
    : IRequestHandler<CreateApplicantCommand, Guid>
{

    public async Task<Guid> Handle(CreateApplicantCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Applicant>(request);

        context.Applicants.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}