using ASD.Onboard.Infrastructure.Identity.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ASD.Onboard.Infrastructure.Identity.OptionSetup;

internal sealed class JwtOptions(IConfiguration configuration)
    : IConfigureOptions<JwtSettings>
{
    public void Configure(JwtSettings jwtSettings)
    {
        configuration.GetSection(JwtSettings.Jwt).Bind(jwtSettings);
    }
}

