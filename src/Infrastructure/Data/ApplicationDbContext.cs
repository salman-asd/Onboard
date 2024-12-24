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
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options), IApplicationDbContext
{

    #region  Applicant Profile
    public DbSet<Applicant> Applicants => Set<Applicant>();
    public DbSet<ApplicantEducation> ApplicantEducations => Set<ApplicantEducation>();
    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Rename ASP.NET Identity tables
        builder.Entity<AppUser>().ToTable("Users");
        builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
