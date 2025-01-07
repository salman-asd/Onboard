namespace ASD.Onboard.Infrastructure.Identity;

public class EmailConfirmationToken
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public DateTime ExpiryTime { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual AppUser User { get; set; }

    private EmailConfirmationToken() { } // For EF Core

    public static EmailConfirmationToken Create(string userId, TimeSpan lifetime)
    {
        return new EmailConfirmationToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ExpiryTime = DateTime.Now.Add(lifetime),
            IsUsed = false,
            CreatedAt = DateTime.Now
        };
    }

    public void MarkAsUsed() => IsUsed = true;
}
