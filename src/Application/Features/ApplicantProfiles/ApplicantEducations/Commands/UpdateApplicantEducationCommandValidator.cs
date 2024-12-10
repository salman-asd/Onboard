namespace ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Commands;

public class UpdateApplicantEducationCommandValidator : AbstractValidator<UpsertApplicantEducationCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateApplicantEducationCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        //RuleFor(v => v.IdentificationNo)
        //    .NotEmpty()
        //    .MaximumLength(200)
        //    .MustAsync(BeUniqueTitle)
        //        .WithMessage("'{PropertyName}' must be unique.")
        //        .WithErrorCode("Unique");
    }

    //public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    //{
    //    return await _context.ApplicantEducations
    //        .AllAsync(l => l.FirstName != title, cancellationToken);
    //}
}
