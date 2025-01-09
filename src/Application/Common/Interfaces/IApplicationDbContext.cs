using ASD.Onboard.Domain.Entities;
using ASD.Onboard.Domain.Entities.Jobs;

namespace ASD.Onboard.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    //DbSet<TodoList> TodoLists { get; }

    //DbSet<TodoItem> TodoItems { get; }

    #region  Applicant Profile
    DbSet<Applicant> Applicants { get; }
    DbSet<ApplicantEducation> ApplicantEducations { get; }
    #endregion

    #region PositionPost
    DbSet<PositionPost> PositionPosts { get; }
    DbSet<JobApplication> JobApplications { get; }
    #endregion

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
