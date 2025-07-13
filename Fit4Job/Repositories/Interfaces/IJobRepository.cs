namespace Fit4Job.Repositories.Interfaces
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        Task<IEnumerable<Job>> GetActiveJobsAsync();
        Task<IEnumerable<Job>> GetJobsByCompanyIdAsync(int companyId);
        Task<IEnumerable<Job>> GetActiveJobsByTypeAsync(JobType jobType);
        Task<IEnumerable<Job>> GetJobsByWorkLocationTypeAsync(WorkLocationType workLocationType);
        Task<IEnumerable<Job>> SearchJobsAsync(string keyword);
        Task<IEnumerable<Job>> GetRecentJobsAsync(int count = 10);
        Task<IEnumerable<Job>> GetDeletedJobsAsync();
        Task<bool> SoftDeleteJobAsync(int jobId);
        Task<bool> RestoreJobAsync(int jobId);
        Task<Job?> ActivateJobAsync(int jobId);

    }
}

         
  