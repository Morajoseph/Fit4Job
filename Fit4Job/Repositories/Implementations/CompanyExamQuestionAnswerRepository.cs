namespace Fit4Job.Repositories.Implementations
{
    public class CompanyExamQuestionAnswerRepository : GenericRepository<CompanyExamQuestionAnswer>, ICompanyExamQuestionAnswerRepository
    {
        public CompanyExamQuestionAnswerRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<CompanyExamQuestionAnswer>> GetAnswersByAttemptIdAsync(int attemptId)
        {
            return await _context.Set<CompanyExamQuestionAnswer>()
                .Where(a => a.AttemptId == attemptId)
                .OrderBy(a => a.QuestionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestionAnswer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await _context.Set<CompanyExamQuestionAnswer>()
                .Where(a => a.QuestionId == questionId)
                .OrderByDescending(a => a.AnsweredAt)
                .ToListAsync();
        }

        public async Task<CompanyExamQuestionAnswer?> GetAnswerByAttemptAndQuestionAsync(int attemptId, int questionId)
        {
            return await _context.Set<CompanyExamQuestionAnswer>()
                .FirstOrDefaultAsync(a => a.AttemptId == attemptId && a.QuestionId == questionId);
        }

        public async Task<IEnumerable<CompanyExamQuestionAnswer>> GetCorrectAnswersByAttemptIdAsync(int attemptId)
        {
            return await _context.Set<CompanyExamQuestionAnswer>()
                .Where(a => a.AttemptId == attemptId && a.IsCorrect)
                .OrderBy(a => a.QuestionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestionAnswer>> GetIncorrectAnswersByAttemptIdAsync(int attemptId)
        {
            return await _context.Set<CompanyExamQuestionAnswer>()
                .Where(a => a.AttemptId == attemptId && !a.IsCorrect)
                .OrderBy(a => a.QuestionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestionAnswer>> GetAnswersWithDetailsAsync(int attemptId)
        {
            return await _context.Set<CompanyExamQuestionAnswer>()
                .Include(a => a.Attempt)
                .Include(a => a.Question)
                .Where(a => a.AttemptId == attemptId)
                .OrderBy(a => a.QuestionId)
                .ToListAsync();
        }

        public async Task<bool> AnswerExistsAsync(int attemptId, int questionId)
        {
            return await _context.Set<CompanyExamQuestionAnswer>()
                .AnyAsync(a => a.AttemptId == attemptId && a.QuestionId == questionId);
        }

        public async Task UpdateAnswerScoreAsync(int answerId, decimal pointsEarned, bool isCorrect)
        {
            var answer = await GetByIdAsync(answerId);
            if (answer != null)
            {
                answer.PointsEarned = pointsEarned;
                answer.IsCorrect = isCorrect;
                Update(answer);
            }
        }

        public async Task<IEnumerable<int>> GetUnansweredQuestionIdsAsync(int attemptId, IEnumerable<int> allQuestionIds)
        {
            var answeredQuestionIds = await _context.Set<CompanyExamQuestionAnswer>()
                .Where(a => a.AttemptId == attemptId)
                .Select(a => a.QuestionId)
                .ToListAsync();

            return allQuestionIds.Except(answeredQuestionIds);
        }

        public async Task<IEnumerable<CompanyExamQuestionAnswer>> GetTextAnswersAsync(int attemptId)
        {
            return await _context.Set<CompanyExamQuestionAnswer>()
                .Where(a => a.AttemptId == attemptId && !string.IsNullOrEmpty(a.TextAnswer))
                .OrderBy(a => a.QuestionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestionAnswer>> GetMultipleChoiceAnswersAsync(int attemptId)
        {
            return await _context.Set<CompanyExamQuestionAnswer>()
                .Where(a => a.AttemptId == attemptId && !string.IsNullOrEmpty(a.SelectedOptionsJson))
                .OrderBy(a => a.QuestionId)
                .ToListAsync();
        }
    }
}