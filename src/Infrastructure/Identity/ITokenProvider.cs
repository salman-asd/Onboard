namespace ASD.Onboard.Infrastructure.Identity;

public interface ITokenProvider
{
    string GenerateAccessToken(string userId, string email, IList<string> roles);
}
