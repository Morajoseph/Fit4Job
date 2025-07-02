namespace Fit4Job.Repositories.Implementations
{
    public class CompanyTaskSubmissionRepository : GenericRepository<CompanyTaskSubmission>, ICompanyTaskSubmissionRepository
    {
        public CompanyTaskSubmissionRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<CompanyTaskSubmission>> GetSubmissionsByTaskIdAsync(int taskId)
        {
            return await _context.CompanyTaskSubmissions
                .Where(s => s.TaskId == taskId)
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTaskSubmission>> GetSubmissionsByUserIdAsync(int userId)
        {
            return await _context.CompanyTaskSubmissions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();
        }

        public async Task<CompanyTaskSubmission?> GetSubmissionByTaskAndUserAsync(int taskId, int userId)
        {
            return await _context.CompanyTaskSubmissions
                .FirstOrDefaultAsync(s => s.TaskId == taskId && s.UserId == userId);
        }

        public async Task<IEnumerable<CompanyTaskSubmission>> GetRecentSubmissionsForTaskAsync(int taskId, int limit)
        {
            return await _context.CompanyTaskSubmissions
                .Where(s => s.TaskId == taskId)
                .OrderByDescending(s => s.SubmittedAt)
                .Take(limit)
                .ToListAsync();
        }
    }
}

