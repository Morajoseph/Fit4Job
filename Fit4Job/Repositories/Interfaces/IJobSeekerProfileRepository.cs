using Fit4Job.ViewModels.JobSeekerProfileViewModels;

namespace Fit4Job.Repositories.Interfaces
{
    public interface IJobSeekerProfileRepository : IGenericRepository<JobSeekerProfile>
    {
        Task<JobSeekerProfile?> GetByUserIdAsync(int userId);
        Task<IEnumerable<JobSeekerProfile>> SearchJobSeekersByNameAsync(string name);
        Task<IEnumerable<JobSeekerProfile>> GetJobSeekersByLocationAsync(string location);
        Task<IEnumerable<JobSeekerProfile>> GetJobSeekersBySkillIdAsync(int skillId);
        Task<IEnumerable<JobSeekerProfile>> GetTopExperiencedJobSeekersAsync(int topN);
        Task<JobSeekerProfile?> GetWithUserByUserIdAsync(int userId);
        Task<JobSeekerProfile?> GetWithUserByIdAsync(int id);
        Task<PagedResultViewModel<JobSeekerProfile>> GetFilteredProfilesAsync(string? location, int? experienceYears, string? currentPosition, int page, int pageSize);



    }
}
