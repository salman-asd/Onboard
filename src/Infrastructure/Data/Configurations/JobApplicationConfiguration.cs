using ASD.Onboard.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASD.Onboard.Infrastructure.Data.Configurations;

internal sealed class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        // Configure ApplicantId as required
        entity.Property(x => x.ApplicantId)
              .IsRequired();

        // Configure PositionPostId as required
        entity.Property(x => x.PositionPostId)
              .IsRequired();

        entity.Property(x => x.Status)
              .HasConversion<int>() // Converts enum to string in the database
              .IsRequired()
              .HasDefaultValue(ApplicationStatus.Applied);

        // Configure indexes (optional, for performance)
        entity.HasIndex(x => x.ApplicantId)
              .HasDatabaseName("IX_JobApplication_ApplicantId");

        entity.HasIndex(x => x.PositionPostId)
              .HasDatabaseName("IX_JobApplication_PositionPostId");
    }
}
