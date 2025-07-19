using Fit4Job.ViewModels.JobSeekerProfileViewModels;

namespace Fit4Job.Repositories.Implementations
{
    public class JobSeekerProfileRepository : GenericRepository<JobSeekerProfile>, IJobSeekerProfileRepository
    {
        public JobSeekerProfileRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public override async Task<JobSeekerProfile?> GetByIdAsync(int id)
        {
            return await _context.JobSeekerProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<JobSeekerProfile>> GetAllAsync()
        {
            return await _context.JobSeekerProfiles
                .Include(p => p.User)
                .OrderBy(p => p.FirstName)
                .ToListAsync();
        }

        public async Task<JobSeekerProfile?> GetByUserIdAsync(int userId)
        {
            return await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(jp => jp.UserId == userId);
        }

        public async Task<JobSeekerProfile?> GetProfileByUserIdAsync(int userId)
        {
            return await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<IEnumerable<JobSeekerProfile>> SearchJobSeekersByNameAsync(string name)
        {
            return await _context.JobSeekerProfiles
                .Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name))
                .OrderBy(p => p.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobSeekerProfile>> GetJobSeekersByLocationAsync(string location)
        {
            return await _context.JobSeekerProfiles
                .Where(p => p.Location.Contains(location))
                .OrderBy(p => p.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobSeekerProfile>> GetJobSeekersBySkillIdAsync(int skillId)
        {
            return await _context.JobSeekerProfiles
                .Where(p => _context.UserSkills
                .Any(us => us.UserId == p.UserId && us.SkillId == skillId))
                .OrderBy(p => p.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobSeekerProfile>> GetTopExperiencedJobSeekersAsync(int topN)
        {
            return await _context.JobSeekerProfiles
                .OrderByDescending(p => p.ExperienceYears)
                .Take(topN)
                .ToListAsync();
        }

        public async Task<PagedResultViewModel<JobSeekerProfile>> GetFilteredProfilesAsync(string? location, int? experienceYears, string? currentPosition, int page, int pageSize)
        {
            var query = _context.JobSeekerProfiles
                .Include(p => p.User)
                .Where(p => p.DeletedAt == null);

            if (!string.IsNullOrEmpty(location))
                query = query.Where(p => p.Location != null && p.Location.Contains(location));

            if (experienceYears.HasValue)
                query = query.Where(p => p.ExperienceYears >= experienceYears.Value);

            if (!string.IsNullOrEmpty(currentPosition))
                query = query.Where(p => p.CurrentPosition != null && p.CurrentPosition.Contains(currentPosition));

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultViewModel<JobSeekerProfile>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<JobSeekerProfile?> GetWithUserByIdAsync(int id)
        {
            return await _context.JobSeekerProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<JobSeekerProfile?> GetWithUserByUserIdAsync(int userId)
        {
            return await _context.JobSeekerProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }






    }
}
