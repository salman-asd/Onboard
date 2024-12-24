namespace ASD.Onboard.Infrastructure.Identity.Options;

internal sealed class JwtSettings
{
    public const string Jwt = "JwtSettings";

    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryMinutes { get; set; }
}
