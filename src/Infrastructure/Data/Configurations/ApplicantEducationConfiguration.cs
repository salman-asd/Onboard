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
            .IsRequired(false);

        builder.Property(ae => ae.InstituteName)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(ae => ae.CountryOfInstitute)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(ae => ae.MajorSubject)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(ae => ae.PassingYear)
            .IsRequired(false);

        builder.Property(ae => ae.Result)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(ae => ae.ResultScale)
           .IsRequired(false)
           .HasColumnType("decimal(3, 2)");

        builder.HasOne(ae => ae.Applicant)
            .WithMany(a => a.ApplicantEducations)
            .HasForeignKey(ae => ae.ApplicantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
