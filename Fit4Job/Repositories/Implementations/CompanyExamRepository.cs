namespace Fit4Job.Repositories.Implementations
{
    public class CompanyExamRepository : GenericRepository<CompanyExam>, ICompanyExamRepository
    {
        public CompanyExamRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<CompanyExam>> GetActiveExamsByCompanyIdAsync(int companyId)
        {
            return await _context.Set<CompanyExam>()
                .Where(e => e.CompanyId == companyId && e.IsActive && e.DeletedAt == null)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExam>> GetAvailableExamsByCompanyIdAsync(int companyId)
        {
            var now = DateTime.UtcNow;
            return await _context.Set<CompanyExam>()
                .Where(e => e.CompanyId == companyId &&
                           e.IsActive &&
                           e.DeletedAt == null &&
                           (e.StartDate == null || e.StartDate <= now) &&
                           (e.EndDate == null || e.EndDate >= now))
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<CompanyExam?> GetExamWithQuestionsAsync(int examId)
        {
            return await _context.Set<CompanyExam>()
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.Id == examId && e.DeletedAt == null);
        }

        public async Task<IEnumerable<CompanyExam>> GetRunningExamsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Set<CompanyExam>()
                .Where(e => e.IsActive &&
                           e.DeletedAt == null &&
                           (e.StartDate == null || e.StartDate <= now) &&
                           (e.EndDate == null || e.EndDate >= now))
                .OrderBy(e => e.EndDate)
                .ToListAsync();
        }

        public async Task<bool> SoftDeleteAsync(int examId)
        {
            var exam = await GetByIdAsync(examId);
            if (exam != null && exam.DeletedAt == null)
            {
                exam.DeletedAt = DateTime.UtcNow;
                exam.UpdatedAt = DateTime.UtcNow;
                exam.IsActive = false;
                Update(exam);
                return true;
            }
            return false;
        }

        public async Task<bool> RestoreAsync(int examId)
        {
            var exam = await _context.Set<CompanyExam>()
                .FirstOrDefaultAsync(e => e.Id == examId);

            if (exam != null && exam.DeletedAt != null)
            {
                exam.DeletedAt = null;
                exam.UpdatedAt = DateTime.UtcNow;
                Update(exam);
                return true;
            }
            return false;
        }

        // Override the base GetByIdAsync to exclude soft deleted records
        public override async Task<CompanyExam?> GetByIdAsync(int id)
        {
            return await _context.Set<CompanyExam>()
                .FirstOrDefaultAsync(e => e.Id == id && e.DeletedAt == null);
        }

        // Override the base GetAllAsync to exclude soft deleted records
        public override async Task<IEnumerable<CompanyExam>> GetAllAsync()
        {
            return await _context.Set<CompanyExam>()
                .Where(e => e.DeletedAt == null)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }
    }
}
