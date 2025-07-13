namespace Fit4Job.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IAdminProfileRepository AdminProfileRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
        IBadgeRepository BadgeRepository { get; }
        ICompanyExamAttemptRepository CompanyExamAttemptRepository { get; }
        ICompanyExamQuestionAnswerRepository CompanyExamQuestionAnswerRepository { get; }
        ICompanyExamQuestionOptionRepository CompanyExamQuestionOptionRepository { get; }
        ICompanyExamQuestionRepository CompanyExamQuestionRepository { get; }
        ICompanyExamRepository CompanyExamRepository { get; }
        ICompanyProfileRepository CompanyProfileRepository { get; }
        ICompanyTaskRepository CompanyTaskRepository { get; }
        ICompanyTaskSubmissionRepository CompanyTaskSubmissionRepository { get; }
        IJobSeekerProfileRepository JobSeekerProfileRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        ISkillRepository SkillRepository { get; }
        ITrackAttemptRepository TrackAttemptRepository { get; }
        ITrackCategoryRepository TrackCategoryRepository { get; }
        ITrackQuestionAnswerRepository TrackQuestionAnswerRepository { get; }
        ITrackQuestionOptionRepository TrackQuestionOptionRepository { get; }
        ITrackQuestionRepository TrackQuestionRepository { get; }
        ITrackRepository TrackRepository { get; }
        IUserBadgeRepository UserBadgeRepository { get; }
        IUserSkillRepository UserSkillRepository { get; }
        IJobRepository JobRepository { get; }
        IJobApplicationRepository JobApplicationRepository { get; }
        Task<int> CompleteAsync();
        Task RollbackAsync();
    }
}
