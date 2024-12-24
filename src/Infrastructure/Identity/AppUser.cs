using Microsoft.AspNetCore.Identity;

namespace ASD.Onboard.Infrastructure.Identity;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
