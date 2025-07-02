namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyTaskSubmissionRepository : IGenericRepository<CompanyTaskSubmission>
    {
        Task<IEnumerable<CompanyTaskSubmission>> GetSubmissionsByTaskIdAsync(int taskId);
        Task<IEnumerable<CompanyTaskSubmission>> GetSubmissionsByUserIdAsync(int userId);
        Task<CompanyTaskSubmission?> GetSubmissionByTaskAndUserAsync(int taskId, int userId);
        Task<IEnumerable<CompanyTaskSubmission>> GetRecentSubmissionsForTaskAsync(int taskId, int limit);
    }
}
