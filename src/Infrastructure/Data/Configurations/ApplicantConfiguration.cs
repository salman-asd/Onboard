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
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.PreferredName)
            .HasMaxLength(100);

        builder.Property(a => a.DOB)
            .IsRequired();

        builder.Property(a => a.BloodGroupId)
            .IsRequired();

        builder.Property(a => a.ReligionId)
            .IsRequired();

        builder.Property(a => a.MaritalStatusId)
            .IsRequired();

        builder.Property(a => a.GenderId)
            .IsRequired();

        builder.Property(a => a.PrimaryMobileNo)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(a => a.SecondaryMobileNo)
            .HasMaxLength(15);

        builder.Property(a => a.PrimaryEmail)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.SecondaryEmail)
            .HasMaxLength(100);

        builder.Property(a => a.Nationality)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.IdentificationType)
            .IsRequired();

        builder.Property(a => a.IdentificationNo)
            .IsRequired();

        builder.Property(a => a.PermAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.PermDistrictId)
            .IsRequired();

        builder.Property(a => a.PermZipCode)
            .IsRequired();

        builder.Property(a => a.PresAddress)
            .HasMaxLength(500);

        builder.Property(a => a.PresDistrictId);

        builder.Property(a => a.PresZipCode);

        builder.Property(a => a.ContactAddress)
            .IsRequired();

        builder.HasMany(a => a.ApplicantEducations)
            .WithOne(ae => ae.Applicant)
            .HasForeignKey(ae => ae.ApplicantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
