namespace Fit4Job.Repositories.Implementations
{
    public class JobSeekerProfileRepository : GenericRepository<JobSeekerProfile>, IJobSeekerProfileRepository
    {
        public JobSeekerProfileRepository(Fit4JobDbContext context) : base(context)
        {

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
    }
}
