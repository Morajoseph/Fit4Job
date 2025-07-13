namespace Fit4Job.Repositories.Implementations
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        public JobRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Job>> GetActiveJobsAsync()
        {
            return await _context.Jobs
                .Where(j => j.IsActive && j.DeletedAt == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetJobsByTypeAsync(JobType jobType)
        {
            return await _context.Jobs
                .Where(j => j.JobType == jobType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetJobsByCompanyIdAsync(int companyId)
        {
            return await _context.Jobs
                .Where(j => j.CompanyId == companyId && j.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetRecentActiveJobsAsync(int count = 10)
        {
            return await _context.Jobs
                .Where(j => j.IsActive && j.DeletedAt == null)
                .OrderByDescending(j => j.CreatedAt)
                .Take(count)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> SearchJobsAsync(string keyword)
        {
            keyword = keyword.ToLower();
            return await _context.Jobs
                .Where(j => (j.Title.ToLower().Contains(keyword) || j.Summary.ToLower().Contains(keyword)) && j.IsActive && j.DeletedAt == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> SoftDeleteJobAsync(int jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null || job.DeletedAt != null)
            {
                return false;
            }
            job.DeletedAt = DateTime.UtcNow;
            return true;
        }

        public async Task<bool> RestoreJobAsync(int jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null || job.DeletedAt == null)
            {
                return false;
            }
            job.DeletedAt = null;
            return true;
        }
    }
}
