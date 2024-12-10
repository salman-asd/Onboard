namespace ASD.Onboard.Application.Features.ApplicantProfiles.ApplicantEducations.Commands;

public class CreateApplicantEducationCommandValidator : AbstractValidator<CreateApplicantEducationCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateApplicantEducationCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        //RuleFor(v => v.FirstName)
        //    .NotEmpty()
        //    .MaximumLength(200)
        //    .MustAsync(BeUniqueTitle)
        //        .WithMessage("'{PropertyName}' must be unique.")
        //        .WithErrorCode("Unique");

        //RuleFor(v => v.LastName)
        //    .NotEmpty()
        //    .MaximumLength(200)
        //    .MustAsync(BeUniqueTitle)
        //        .WithMessage("'{PropertyName}' must be unique.")
        //        .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _context.ApplicantEducations
            .AllAsync(l => l.InstituteName != title, cancellationToken);
    }
}
