namespace Fit4Job.Repositories.Implementations
{
    public class CompanyExamQuestionOptionRepository : GenericRepository<CompanyExamQuestionOption>, ICompanyExamQuestionOptionRepository
    {
        public CompanyExamQuestionOptionRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<CompanyExamQuestionOption>> GetOptionsByQuestionIdAsync(int questionId)
        {
            return await _context.Set<CompanyExamQuestionOption>()
                .Where(o => o.QuestionId == questionId)
                .OrderBy(o => o.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestionOption>> GetActiveOptionsByQuestionIdAsync(int questionId)
        {
            return await _context.Set<CompanyExamQuestionOption>()
                .Where(o => o.QuestionId == questionId && o.DeletedAt == null)
                .OrderBy(o => o.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestionOption>> GetCorrectOptionsByQuestionIdAsync(int questionId)
        {
            return await _context.Set<CompanyExamQuestionOption>()
                .Where(o => o.QuestionId == questionId && o.IsCorrect && o.DeletedAt == null)
                .OrderBy(o => o.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestionOption>> GetIncorrectOptionsByQuestionIdAsync(int questionId)
        {
            return await _context.Set<CompanyExamQuestionOption>()
                .Where(o => o.QuestionId == questionId && !o.IsCorrect && o.DeletedAt == null)
                .OrderBy(o => o.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestionOption>> GetOptionsWithDetailsAsync(int questionId)
        {
            return await _context.Set<CompanyExamQuestionOption>()
                .Include(o => o.Question)
                .Where(o => o.QuestionId == questionId)
                .OrderBy(o => o.Id)
                .ToListAsync();
        }

        public async Task<CompanyExamQuestionOption?> GetOptionWithQuestionAsync(int optionId)
        {
            return await _context.Set<CompanyExamQuestionOption>()
                .Include(o => o.Question)
                .FirstOrDefaultAsync(o => o.Id == optionId);
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

        public async Task<int> GetOptionsCountAsync(int questionId)
        {
            return await _context.Set<CompanyExamQuestionOption>()
                .CountAsync(o => o.QuestionId == questionId && o.DeletedAt == null);
        }

        public async Task<int> GetCorrectOptionsCountAsync(int questionId)
        {
            return await _context.Set<CompanyExamQuestionOption>()
                .CountAsync(o => o.QuestionId == questionId && o.IsCorrect && o.DeletedAt == null);
        }

        public async Task<bool> AllOptionsBelongToQuestionAsync(IEnumerable<int> optionIds, int questionId)
        {
            var optionCount = await _context.Set<CompanyExamQuestionOption>()
                .CountAsync(o => optionIds.Contains(o.Id) &&
                                o.QuestionId == questionId &&
                                o.DeletedAt == null);

            return optionCount == optionIds.Count();
        }

        public async Task UpdateOptionCorrectnessAsync(int optionId, bool isCorrect)
        {
            var option = await GetByIdAsync(optionId);
            if (option != null)
            {
                option.IsCorrect = isCorrect;
                Update(option);
            }
        }

        public async Task BulkUpdateOptionsAsync(int questionId, IEnumerable<CompanyExamQuestionOption> options)
        {
            var existingOptions = await _context.Set<CompanyExamQuestionOption>()
                .Where(o => o.QuestionId == questionId)
                .ToListAsync();

            foreach (var option in options)
            {
                var existingOption = existingOptions.FirstOrDefault(o => o.Id == option.Id);
                if (existingOption != null)
                {
                    existingOption.OptionText = option.OptionText;
                    existingOption.IsCorrect = option.IsCorrect;
                    Update(existingOption);
                }
            }
        }
    }
}
