namespace ASD.Onboard.Application.Common.Interfaces;

public interface IApplicantService
{
    Task<Guid?> GetApplicantIdAsync(CancellationToken cancellationToken = default);
}
