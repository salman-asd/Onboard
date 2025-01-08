using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ASD.Onboard.Domain.Entities.Applicants;

public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
{
    public void Configure(EntityTypeBuilder<Applicant> builder)
    {
        builder.ToTable("Applicants");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.FirstName)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(a => a.LastName)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(a => a.PreferredName)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(a => a.DOB)
            .IsRequired(false);

        builder.Property(a => a.BloodGroupId)
            .IsRequired(false);

        builder.Property(a => a.ReligionId)
            .IsRequired(false);

        builder.Property(a => a.MaritalStatusId)
            .IsRequired(false);

        builder.Property(a => a.GenderId)
            .IsRequired(false);

        builder.Property(a => a.PrimaryMobileNo)
            .IsRequired(false)
            .HasMaxLength(15);

        builder.Property(a => a.SecondaryMobileNo)
            .IsRequired(false)
            .HasMaxLength(15);

        builder.Property(a => a.PrimaryEmail)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(a => a.SecondaryEmail)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(a => a.Nationality)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(a => a.IdentificationType)
            .IsRequired(false);

        builder.Property(a => a.IdentificationNo)
            .IsRequired(false);

        builder.Property(a => a.PermAddress)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(a => a.PermDistrictId)
            .IsRequired(false);

        builder.Property(a => a.PermZipCode)
            .IsRequired(false);

        builder.Property(a => a.PresAddress)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(a => a.PresDistrictId)
            .IsRequired(false);

        builder.Property(a => a.PresZipCode)
            .IsRequired(false);

        builder.Property(a => a.ContactAddress)
            .HasDefaultValue(1);

        builder.HasMany(a => a.ApplicantEducations)
            .WithOne(ae => ae.Applicant)
            .HasForeignKey(ae => ae.ApplicantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
