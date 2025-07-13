namespace Fit4Job.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Fit4JobDbContext _context;

        public IAdminProfileRepository AdminProfileRepository { get; }

        public IApplicationUserRepository ApplicationUserRepository { get; }

        public IBadgeRepository BadgeRepository { get; }

        public ICompanyExamAttemptRepository CompanyExamAttemptRepository { get; }

        public ICompanyExamQuestionAnswerRepository CompanyExamQuestionAnswerRepository { get; }

        public ICompanyExamQuestionOptionRepository CompanyExamQuestionOptionRepository { get; }

        public ICompanyExamQuestionRepository CompanyExamQuestionRepository { get; }

        public ICompanyExamRepository CompanyExamRepository { get; }

        public ICompanyProfileRepository CompanyProfileRepository { get; }

        public ICompanyTaskRepository CompanyTaskRepository { get; }

        public ICompanyTaskSubmissionRepository CompanyTaskSubmissionRepository { get; }

        public IJobSeekerProfileRepository JobSeekerProfileRepository { get; }

        public INotificationRepository NotificationRepository { get; }

        public IPaymentRepository PaymentRepository { get; }

        public ISkillRepository SkillRepository { get; }

        public ITrackAttemptRepository TrackAttemptRepository { get; }

        public ITrackCategoryRepository TrackCategoryRepository { get; }

        public ITrackQuestionAnswerRepository TrackQuestionAnswerRepository { get; }

        public ITrackQuestionOptionRepository TrackQuestionOptionRepository { get; }

        public ITrackQuestionRepository TrackQuestionRepository { get; }

        public ITrackRepository TrackRepository { get; }

        public IUserBadgeRepository UserBadgeRepository { get; }

        public IUserSkillRepository UserSkillRepository { get; }
        public IJobRepository JobRepository {  get; }
        public IJobApplicationRepository JobApplicationRepository { get; }

        public UnitOfWork(Fit4JobDbContext context)
        {
            _context = context;
            AdminProfileRepository = new AdminProfileRepository(_context);
            ApplicationUserRepository = new ApplicationUserRepository(_context);
            BadgeRepository = new BadgeRepository(_context);
            CompanyExamAttemptRepository = new CompanyExamAttemptRepository(_context);
            CompanyExamQuestionAnswerRepository = new CompanyExamQuestionAnswerRepository(_context);
            CompanyExamQuestionOptionRepository = new CompanyExamQuestionOptionRepository(_context);
            CompanyExamQuestionRepository = new CompanyExamQuestionRepository(_context);
            CompanyExamRepository = new CompanyExamRepository(_context);
            CompanyProfileRepository = new CompanyProfileRepository(_context);
            CompanyTaskRepository = new CompanyTaskRepository(_context);
            CompanyTaskSubmissionRepository = new CompanyTaskSubmissionRepository(_context);
            JobSeekerProfileRepository = new JobSeekerProfileRepository(_context);
            NotificationRepository = new NotificationRepository(_context);
            PaymentRepository = new PaymentRepository(_context);
            SkillRepository = new SkillRepository(_context);
            TrackAttemptRepository = new TrackAttemptRepository(_context);
            TrackCategoryRepository = new TrackCategoryRepository(_context);
            TrackQuestionAnswerRepository = new TrackQuestionAnswerRepository(_context);
            TrackQuestionOptionRepository = new TrackQuestionOptionRepository(_context);
            TrackQuestionRepository = new TrackQuestionRepository(_context);
            TrackRepository = new TrackRepository(_context);
            UserBadgeRepository = new UserBadgeRepository(_context);
            UserSkillRepository = new UserSkillRepository(_context);
            JobRepository=new JobRepository(_context);
            JobApplicationRepository = new JobApplicationRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
