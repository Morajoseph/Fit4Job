namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyExamAttemptRepository : IGenericRepository<CompanyExamAttempt>
    {
        Task<IEnumerable<CompanyExamAttempt>> GetAttemptsByExamIdAsync(int examId);
        Task<CompanyExamAttempt?> GetAttemptWithAnswersAsync(int attemptId);
        Task<CompanyExamAttempt?> GetAttemptWithFullDetailsAsync(int attemptId);
        Task<IEnumerable<CompanyExamAttempt>> GetTopPerformersAsync(int examId, int topCount = 10);
        Task<bool> SoftDeleteAsync(int attemptId);
        Task<bool> RestoreAsync(int attemptId);
        Task<bool> CompleteAttemptAsync(int attemptId, decimal finalScore, bool passed);
    }
}
