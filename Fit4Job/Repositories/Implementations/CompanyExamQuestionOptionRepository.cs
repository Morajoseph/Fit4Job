namespace Fit4Job.Repositories.Implementations
{
    public class CompanyExamQuestionOptionRepository : GenericRepository<CompanyExamQuestionOption>, ICompanyExamQuestionOptionRepository
    {
        public CompanyExamQuestionOptionRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<CompanyExamQuestionOption>> GetOptionsByQuestionIdAsync(int questionId)
        {
            return await _context.CompanyExamQuestionOptions
                .Where(o => o.QuestionId == questionId)
                .OrderBy(o => o.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestionOption>> GetActiveOptionsByQuestionIdAsync(int questionId)
        {
            return await _context.CompanyExamQuestionOptions
                .Where(o => o.QuestionId == questionId && o.DeletedAt == null)
                .OrderBy(o => o.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestionOption>> GetCorrectOptionsByQuestionIdAsync(int questionId)
        {
            return await _context.CompanyExamQuestionOptions
                .Where(o => o.QuestionId == questionId && o.IsCorrect && o.DeletedAt == null)
                .OrderBy(o => o.Id)
                .ToListAsync();
        }

        public async Task<int> GetOptionsCountAsync(int questionId)
        {
            return await _context.CompanyExamQuestionOptions
                .CountAsync(o => o.QuestionId == questionId && o.DeletedAt == null);
        }

        public async Task<int> GetCorrectOptionsCountAsync(int questionId)
        {
            return await _context.CompanyExamQuestionOptions
                .CountAsync(o => o.QuestionId == questionId && o.IsCorrect && o.DeletedAt == null);
        }

        public async Task<bool> SoftDeleteOptionAsync(int optionId)
        {
            var option = await GetByIdAsync(optionId);
            if (option != null && option.DeletedAt == null)
            {
                option.DeletedAt = DateTime.UtcNow;
                Update(option);
                return true;
            }
            return false;
        }

        public async Task<bool> RestoreOptionAsync(int optionId)
        {
            var option = await GetByIdAsync(optionId);
            if (option != null && option.DeletedAt != null)
            {
                option.DeletedAt = null;
                Update(option);
                return true;
            }
            return false;
        }
    }
}
