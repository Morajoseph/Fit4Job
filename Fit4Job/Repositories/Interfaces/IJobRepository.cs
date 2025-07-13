namespace Fit4Job.Repositories.Interfaces
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        Task<IEnumerable<Job>> GetActiveJobsAsync();
        Task<IEnumerable<Job>> GetJobsByCompanyIdAsync(int companyId);
        Task<IEnumerable<Job>> GetJobsByTypeAsync(JobType jobType);
        Task<IEnumerable<Job>> SearchJobsAsync(string keyword);
        Task<IEnumerable<Job>> GetRecentActiveJobsAsync(int count = 10);
        Task<bool> SoftDeleteJobAsync(int jobId);
        Task<bool> RestoreJobAsync(int jobId);
    }
}