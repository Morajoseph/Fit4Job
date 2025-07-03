namespace Fit4Job.Repositories.Implementations
{
    public class CompanyExamQuestionRepository : GenericRepository<CompanyExamQuestion>, ICompanyExamQuestionRepository
    {
        public CompanyExamQuestionRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<CompanyExamQuestion>> GetByExamIdAsync(int examId)
        {
            return await _context.CompanyExamQuestions
                .Where(q => q.ExamId == examId)
                .OrderBy(q => q.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestion>> GetActiveQuestionsByExamIdAsync(int examId)
        {
            return await _context.CompanyExamQuestions
                .Where(q => q.ExamId == examId && q.DeletedAt == null)
                .OrderBy(q => q.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestion>> GetByExamIdAndTypeAsync(int examId, QuestionType questionType)
        {
            return await _context.CompanyExamQuestions
                .Where(q => q.ExamId == examId && q.QuestionType == questionType && q.DeletedAt == null)
                .OrderBy(q => q.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestion>> GetQuestionsWithOptionsAsync(int examId)
        {
            return await _context.CompanyExamQuestions
                .Where(q => q.ExamId == examId && q.DeletedAt == null)
                .Include(q => q.Options)
                .OrderBy(q => q.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestion>> GetQuestionsWithAnswersAsync(int examId)
        {
            return await _context.CompanyExamQuestions
                .Where(q => q.ExamId == examId && q.DeletedAt == null)
                .Include(q => q.Answers)
                .OrderBy(q => q.Id)
                .ToListAsync();
        }

        public async Task<CompanyExamQuestion?> GetCompleteQuestionAsync(int questionId)
        {
            return await _context.CompanyExamQuestions
                .Where(q => q.Id == questionId && q.DeletedAt == null)
                .Include(q => q.Options)
                .Include(q => q.Answers)
                .Include(q => q.Exam)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> GetTotalPointsByExamIdAsync(int examId)
        {
            return await _context.CompanyExamQuestions
                .Where(q => q.ExamId == examId && q.DeletedAt == null)
                .SumAsync(q => q.Points);
        }

        public async Task<int> CountQuestionsByExamIdAsync(int examId)
        {
            return await _context.CompanyExamQuestions
                .CountAsync(q => q.ExamId == examId && q.DeletedAt == null);
        }

        public async Task<bool> SoftDeleteAsync(int questionId)
        {
            var question = await GetByIdAsync(questionId);
            if (question != null && question.DeletedAt == null)
            {
                question.DeletedAt = DateTime.UtcNow;
                Update(question);
                return true;
            }
            return false;
        }

        public async Task<bool> RestoreAsync(int questionId)
        {
            var question = await _context.CompanyExamQuestions
                .FirstOrDefaultAsync(q => q.Id == questionId && q.DeletedAt != null);

            if (question != null)
            {
                question.DeletedAt = null;
                Update(question);
                return true;
            }
            return false;
        }

        // Override the generic GetByIdAsync to exclude soft deleted records
        public override async Task<CompanyExamQuestion?> GetByIdAsync(int id)
        {
            return await _context.CompanyExamQuestions
                .FirstOrDefaultAsync(q => q.Id == id && q.DeletedAt == null);
        }

        // Override the generic GetAllAsync to exclude soft deleted records
        public override async Task<IEnumerable<CompanyExamQuestion>> GetAllAsync()
        {
            return await _context.CompanyExamQuestions
                .Where(q => q.DeletedAt == null)
                .OrderBy(q => q.CreatedAt)
                .ToListAsync();
        }
    }
}
