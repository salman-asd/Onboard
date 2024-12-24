namespace ASD.Onboard.Application.Features.Identity.Models;

public record RegisterRequest()
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
