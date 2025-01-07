using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ASD.Onboard.Infrastructure.Identity.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ASD.Onboard.Infrastructure.Identity;

internal sealed class TokenProvider(IOptions<JwtSettings> jwtSettings): ITokenProvider
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    public string GenerateAccessToken(string userId, string email, IList<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Name, email.Split('@').First().ToLower())
            };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
