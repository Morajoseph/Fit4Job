namespace Fit4Job.DataContext
{
    public class Fit4JobDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public Fit4JobDbContext()
        {

        }

        public Fit4JobDbContext(DbContextOptions<Fit4JobDbContext> options) : base(options)
        {

        }
        public DbSet<AdminProfile> AdminProfiles { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<CompanyExam> CompanyExams { get; set; }
        public DbSet<CompanyExamAttempt> CompanyExamAttempts { get; set; }
        public DbSet<CompanyExamQuestion> CompanyExamQuestions { get; set; }
        public DbSet<CompanyExamQuestionAnswer> CompanyExamQuestionAnswers { get; set; }
        public DbSet<CompanyExamQuestionOption> CompanyExamQuestionOptions { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public DbSet<CompanyTask> CompanyTasks { get; set; }
        public DbSet<CompanyTaskSubmission> CompanyTaskSubmissions { get; set; }
        public DbSet<JobSeekerProfile> JobSeekerProfiles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackAttempt> TrackAttempts { get; set; }
        public DbSet<TrackCategory> TrackCategories { get; set; }
        public DbSet<TrackQuestion> TrackQuestions { get; set; }
        public DbSet<TrackQuestionAnswer> TrackQuestionAnswers { get; set; }
        public DbSet<TrackQuestionOption> TrackQuestionOptions { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
    }
}