using System.Reflection;
using ASD.Onboard.Application.Common.Interfaces;
using ASD.Onboard.Domain.Entities;
using ASD.Onboard.Domain.Entities.Applicants;
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
    public DbSet<EmailOutboxMessage> EmailOutboxes => Set<EmailOutboxMessage>();
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

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
