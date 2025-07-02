namespace Fit4Job.Repositories.Interfaces
{
    public interface IJobSeekerProfileRepository : IGenericRepository<JobSeekerProfile>
    {
        Task<JobSeekerProfile?> GetProfileByUserIdAsync(int userId);
        Task<IEnumerable<JobSeekerProfile>> SearchJobSeekersByNameAsync(string name);
        Task<IEnumerable<JobSeekerProfile>> GetJobSeekersByLocationAsync(string location);
        Task<IEnumerable<JobSeekerProfile>> GetJobSeekersBySkillIdAsync(int skillId);
        Task<IEnumerable<JobSeekerProfile>> GetTopExperiencedJobSeekersAsync(int topN);
    }
}
