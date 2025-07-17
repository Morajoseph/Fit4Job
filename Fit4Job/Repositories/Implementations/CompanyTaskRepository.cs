namespace Fit4Job.Repositories.Implementations
{
    public class CompanyTaskRepository : GenericRepository<CompanyTask>, ICompanyTaskRepository
    {
        public CompanyTaskRepository(Fit4JobDbContext context) : base(context)
        {

        }
        public async Task<CompanyTask?> GetByJobIdAsync(int jobId)
        {
            return await _context.CompanyTasks
                .Where(t => t.JobId == jobId)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<CompanyTask>> GetTasksByCompanyIdAsync(int companyId)
        {
            return await _context.CompanyTasks
                .Where(t => t.CompanyId == companyId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetActiveTasksAsync()
        {
            return await _context.CompanyTasks
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetTasksByDeadlineAsync(DateTime deadline)
        {
            return await _context.CompanyTasks
                .Where(t => t.Deadline.Date <= deadline.Date)
                .OrderBy(t => t.Deadline)
                .ToListAsync();
        }


    }
}
