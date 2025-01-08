using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Application.Common.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASD.Onboard.Infrastructure.Data;

internal sealed class ApplicantService(
    IApplicationDbContext context, 
    IUser user) : IApplicantService
{
    public async Task<Guid?> GetApplicantIdAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = user.Id?.ToLower();
            if (string.IsNullOrEmpty(userId))
                return null;

            var applicantId = await context.Applicants
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .SingleOrDefaultAsync(cancellationToken);

            return applicantId;
        }
        catch (Exception ex)
        {
            return null;
        }

    }
}
