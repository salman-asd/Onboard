using System.Reflection;
using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Domain.Entities;
using ASD.Onboard.Domain.Entities.Applicants;
using ASD.Onboard.Domain.Entities.Jobs;
using ASD.Onboard.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASD.Onboard.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<AppUser>(options), IApplicationDbContext
{

    #region  Applicant Profile
    public DbSet<Applicant> Applicants => Set<Applicant>();
    public DbSet<ApplicantEducation> ApplicantEducations => Set<ApplicantEducation>();
    #endregion

    #region Position Post
    public DbSet<PositionPost> PositionPosts => Set<PositionPost>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    #endregion

    public DbSet<EmailOutboxMessage> EmailOutboxes => Set<EmailOutboxMessage>();
    public DbSet<EmailConfirmationToken> EmailConfirmationTokens => Set<EmailConfirmationToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Rename ASP.NET Identity tables
        builder.Entity<AppUser>().ToTable("Users");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        ConfigureEmailConfirmationToken(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    private static void ConfigureEmailConfirmationToken(ModelBuilder builder)
    {
        builder.Entity<EmailConfirmationToken>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.UserId)
                .IsRequired();

            entity.Property(t => t.ExpiryTime)
                .IsRequired();

            entity.Property(t => t.IsUsed)
                .IsRequired();

            entity.Property(t => t.CreatedAt)
                .IsRequired();

            // Index for faster lookups
            entity.HasIndex(t => new { t.UserId, t.IsUsed, t.ExpiryTime });

            // Relationship with User
            //entity.HasOne<AppUser>()
            //    .WithMany()
            //    .HasForeignKey(t => t.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);
        });

    }

}


