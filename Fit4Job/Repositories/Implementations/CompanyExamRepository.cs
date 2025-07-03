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

        public async Task<CompanyExam?> GetExamWithFullDetailsAsync(int examId)
        {
            return await _context.Set<CompanyExam>()
                .Include(e => e.Questions)
                .Include(e => e.Attempts)
                .Include(e => e.Company)
                .FirstOrDefaultAsync(e => e.Id == examId && e.DeletedAt == null);
        }

        public async Task<IEnumerable<CompanyExam>> GetExamsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Set<CompanyExam>()
                .Where(e => e.DeletedAt == null &&
                           ((e.StartDate >= startDate && e.StartDate <= endDate) ||
                            (e.EndDate >= startDate && e.EndDate <= endDate) ||
                            (e.StartDate <= startDate && e.EndDate >= endDate)))
                .OrderBy(e => e.StartDate)
                .ToListAsync();
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

        public async Task<IEnumerable<CompanyExam>> GetEndedExamsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Set<CompanyExam>()
                .Where(e => e.DeletedAt == null && e.EndDate.HasValue && e.EndDate < now)
                .OrderByDescending(e => e.EndDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExam>> GetScheduledExamsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Set<CompanyExam>()
                .Where(e => e.IsActive &&
                           e.DeletedAt == null &&
                           e.StartDate.HasValue &&
                           e.StartDate > now)
                .OrderBy(e => e.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExam>> SearchExamsByTitleAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _context.Set<CompanyExam>()
                .Where(e => e.DeletedAt == null &&
                           e.Title.Contains(searchTerm))
                .OrderByDescending(e => e.CreatedAt)
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

        public async Task<bool> ExamTitleExistsAsync(int companyId, string title, int? excludeExamId = null)
        {
            var query = _context.Set<CompanyExam>()
                .Where(e => e.CompanyId == companyId &&
                           e.Title.ToLower() == title.ToLower() &&
                           e.DeletedAt == null);

            if (excludeExamId.HasValue)
            {
                query = query.Where(e => e.Id != excludeExamId.Value);
            }

            return await query.AnyAsync();
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
