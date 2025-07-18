namespace Fit4Job.Repositories.Interfaces
{
    public interface IJobApplicationRepository : IGenericRepository<JobApplication>
    {
        Task<IEnumerable<JobApplication>> GetJobsByUserIdAsync(int userId);
        Task<IEnumerable<JobApplication>> GetAllForJobIdAsync(int jobId);
        Task<IEnumerable<JobApplication>> GetUsersByJobIdAsync(int jobId);
        Task<IEnumerable<JobApplication>> GetApplicationsWithDetailsByJobIdAsync(int jobId);
        Task<JobApplication?> GetByUserAndJobAsync(int userId, int jobId);
        Task<bool> ExistsAsync(int userId, int jobId);
    }
}
