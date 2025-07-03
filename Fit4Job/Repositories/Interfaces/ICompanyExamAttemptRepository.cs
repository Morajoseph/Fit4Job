namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyExamAttemptRepository : IGenericRepository<CompanyExamAttempt>
    {
        // Get attempts by user
        Task<IEnumerable<CompanyExamAttempt>> GetAttemptsByUserIdAsync(int userId);
        // Get attempts by exam
        Task<IEnumerable<CompanyExamAttempt>> GetAttemptsByExamIdAsync(int examId);

        // Get attempt with answers included
        Task<CompanyExamAttempt?> GetAttemptWithAnswersAsync(int attemptId);

        // Get attempt with full details (user, exam, answers)
        Task<CompanyExamAttempt?> GetAttemptWithFullDetailsAsync(int attemptId);

        // Get user's attempts for specific exam
        Task<IEnumerable<CompanyExamAttempt>> GetUserExamAttemptsAsync(int userId, int examId);

        // Get attempts by date range
        Task<IEnumerable<CompanyExamAttempt>> GetAttemptsByDateRangeAsync(DateTime startDate, DateTime endDate);

        // Get top performers for exam
        Task<IEnumerable<CompanyExamAttempt>> GetTopPerformersAsync(int examId, int topCount = 10);

        // Soft delete attempt
        Task<bool> SoftDeleteAsync(int attemptId);

        // Restore soft deleted attempt
        Task<bool> RestoreAsync(int attemptId);

        // Complete attempt (set end time and calculate final score)
        Task<bool> CompleteAttemptAsync(int attemptId, decimal finalScore, bool passed);
    }
}
