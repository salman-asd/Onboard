using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ASD.Onboard.Domain.Entities.Jobs;

namespace ASD.Onboard.Infrastructure.Data.Configurations;

internal sealed class PositionPostConfiguration : IEntityTypeConfiguration<PositionPost>
{
    public void Configure(EntityTypeBuilder<PositionPost> entity)
    {
        entity.ToTable("PositionPosts", "dbo");

        entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

        entity.Property(e => e.CompanyName).HasMaxLength(100);

        entity.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(false);

        entity.Property(e => e.DepartmentName).HasMaxLength(100);

        entity.Property(e => e.DesignationName).HasMaxLength(100);

        entity.Property(e => e.EmploymentStatusName).HasMaxLength(100);

        entity.Property(e => e.Experience).HasColumnType("decimal(4, 2)");

        entity.Property(e => e.JobPostTitle).HasMaxLength(100);

        entity.Property(e => e.LastModifiedBy)
            .HasMaxLength(100)
            .IsUnicode(false);

        entity.Property(e => e.Location).HasMaxLength(100);

        entity.Property(e => e.RefNo).HasMaxLength(50);

        entity.Property(e => e.SalaryRange).HasMaxLength(100);

        entity.Property(e => e.ValidUpTo).HasColumnType("date");
    }
}
