using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ASD.Onboard.Domain.Entities.Applicants;

public class ApplicantEducationConfiguration : IEntityTypeConfiguration<ApplicantEducation>
{
    public void Configure(EntityTypeBuilder<ApplicantEducation> builder)
    {
        builder.ToTable("ApplicantEducations");

        builder.HasKey(ae => ae.Id);

        builder.Property(ae => ae.EducationLevel)
            .IsRequired();

        builder.Property(ae => ae.InstituteName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ae => ae.CountryOfInstitute)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ae => ae.MajorSubject)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ae => ae.PassingYear)
            .IsRequired();

        builder.Property(ae => ae.Result)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(ae => ae.Applicant)
            .WithMany(a => a.ApplicantEducations)
            .HasForeignKey(ae => ae.ApplicantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
