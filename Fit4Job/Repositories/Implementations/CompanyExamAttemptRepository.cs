namespace Fit4Job.Repositories.Implementations
{
    public class CompanyExamAttemptRepository : GenericRepository<CompanyExamAttempt>, ICompanyExamAttemptRepository
    {
        public CompanyExamAttemptRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<CompanyExamAttempt>> GetAttemptsByExamIdAsync(int examId)
        {
            return await _context.CompanyExamAttempts
                .Where(a => a.ExamId == examId && a.DeletedAt == null)
                .Include(a => a.User)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<CompanyExamAttempt?> GetAttemptWithAnswersAsync(int attemptId)
        {
            return await _context.CompanyExamAttempts
                .Include(a => a.Answers)
                .FirstOrDefaultAsync(a => a.Id == attemptId && a.DeletedAt == null);
        }

        public async Task<CompanyExamAttempt?> GetAttemptWithFullDetailsAsync(int attemptId)
        {
            return await _context.CompanyExamAttempts
                .Include(a => a.User)
                .Include(a => a.Exam)
                .Include(a => a.Answers)
                .FirstOrDefaultAsync(a => a.Id == attemptId && a.DeletedAt == null);
        }

        public Task<CompanyExamAttempt?> GetByJobApplicationIdAsync(int jobApplicationId)
        {
            return _context.CompanyExamAttempts
                .Where(a => a.JobApplicationId == jobApplicationId && a.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CompanyExamAttempt>> GetTopPerformersAsync(int examId, int topCount = 10)
        {
            return await _context.CompanyExamAttempts
                .Where(a => a.ExamId == examId && a.DeletedAt == null)
                .Include(a => a.User)
                .OrderByDescending(a => a.Score)
                .ThenBy(a => a.Duration)
                .Take(topCount)
                .ToListAsync();
        }

        public async Task<bool> SoftDeleteAsync(int attemptId)
        {
            var attempt = await GetByIdAsync(attemptId);
            if (attempt != null && attempt.DeletedAt == null)
            {
                attempt.DeletedAt = DateTime.UtcNow;
                attempt.UpdatedAt = DateTime.UtcNow;
                Update(attempt);
                return true;
            }
            return false;
        }

        public async Task<bool> RestoreAsync(int attemptId)
        {
            var attempt = await _context.CompanyExamAttempts
                .FirstOrDefaultAsync(a => a.Id == attemptId);

            if (attempt != null && attempt.DeletedAt != null)
            {
                attempt.DeletedAt = null;
                attempt.UpdatedAt = DateTime.UtcNow;
                Update(attempt);
                return true;
            }
            return false;
        }

        public async Task<bool> CompleteAttemptAsync(int attemptId, decimal finalScore, bool passed)
        {
            var attempt = await GetByIdAsync(attemptId);
            if (attempt != null && attempt.Status == CompanyExamAttemptStatus.InProgress)
            {
                attempt.EndTime = DateTime.UtcNow;
                attempt.Score = finalScore;
                attempt.Passed = passed;
                attempt.Status = CompanyExamAttemptStatus.Completed;
                attempt.UpdatedAt = DateTime.UtcNow;

                // Calculate percentage score
                var exam = await _context.CompanyExams.FindAsync(attempt.ExamId);
                if (exam != null && exam.TotalScore > 0)
                {
                    attempt.PercentageScore = (finalScore / exam.TotalScore) * 100;
                }

                Update(attempt);
                return true;
            }
            return false;
        }

        // Override the base GetByIdAsync to exclude soft deleted records
        public override async Task<CompanyExamAttempt?> GetByIdAsync(int id)
        {
            return await _context.CompanyExamAttempts
                .FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == null);
        }

        // Override the base GetAllAsync to exclude soft deleted records
        public override async Task<IEnumerable<CompanyExamAttempt>> GetAllAsync()
        {
            return await _context.CompanyExamAttempts
                .Where(a => a.DeletedAt == null)
                .Include(a => a.User)
                .Include(a => a.Exam)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

    }
}