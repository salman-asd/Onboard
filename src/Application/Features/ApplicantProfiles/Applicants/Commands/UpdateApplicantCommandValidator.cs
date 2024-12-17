namespace ASD.Onboard.Application.Features.ApplicantProfiles.Applicants.Commands;

public class UpdateApplicantCommandValidator : AbstractValidator<UpdateApplicantCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateApplicantCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        //RuleFor(v => v.FirstName)
        //    .NotEmpty()
        //    .MaximumLength(200)
        //    .MustAsync(BeUniqueTitle)
        //        .WithMessage("'{PropertyName}' must be unique.")
        //        .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _context.Applicants
            .AllAsync(l => l.FirstName != title, cancellationToken);
    }
}
