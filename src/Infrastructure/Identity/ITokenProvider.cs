namespace ASD.Onboard.Infrastructure.Identity;

public interface ITokenProvider
{
    string GenerateAccessToken(AppUser user, IList<string> roles);
}
