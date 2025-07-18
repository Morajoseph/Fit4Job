namespace Fit4Job.Repositories.Implementations
{
    public class JobApplicationRepository : GenericRepository<JobApplication>, IJobApplicationRepository
    {
        public JobApplicationRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<JobApplication>> GetJobsByUserIdAsync(int userId)
        {
            return await _context.JobApplications
                .Where(a => a.UserId == userId)
                .Include(a => a.Job)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobApplication>> GetUsersByJobIdAsync(int jobId)
        {
            return await _context.JobApplications
                .Where(a => a.JobId == jobId)
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobApplication>> GetApplicationsWithDetailsByJobIdAsync(int jobId)
        {
            return await _context.JobApplications
                .Where(a => a.JobId == jobId)
                .Include(a => a.User)
                .Include(a => a.ExamAttempt)
                .Include(a => a.TaskSubmission)
                .ToListAsync();
        }


        public async Task<JobApplication?> GetByUserAndJobAsync(int userId, int jobId)
        {
            return await _context.JobApplications
                .FirstOrDefaultAsync(a => a.UserId == userId && a.JobId == jobId);
        }

        public async Task<bool> ExistsAsync(int userId, int jobId)
        {
            return await _context.JobApplications
                .AnyAsync(a => a.UserId == userId && a.JobId == jobId);
        }

        public async Task<IEnumerable<JobApplication>> GetAllForJobIdAsync(int jobId)
        {
            return await _context.JobApplications
              .Where(a => a.JobId == jobId)
              .Include(a => a.User)
              .ToListAsync();
        }
    }
}
