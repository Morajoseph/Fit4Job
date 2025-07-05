namespace Fit4Job.Repositories.Implementations
{
    public class CompanyExamQuestionAnswerRepository : GenericRepository<CompanyExamQuestionAnswer>, ICompanyExamQuestionAnswerRepository
    {
        public CompanyExamQuestionAnswerRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<CompanyExamQuestionAnswer>> GetAnswersByAttemptIdAsync(int attemptId)
        {
            return await _context.CompanyExamQuestionAnswers
                .Where(a => a.AttemptId == attemptId)
                .OrderBy(a => a.QuestionId)
                .ToListAsync();
        }

        public async Task<CompanyExamQuestionAnswer?> GetAnswerByAttemptAndQuestionAsync(int attemptId, int questionId)
        {
            return await _context.CompanyExamQuestionAnswers
                .FirstOrDefaultAsync(a => a.AttemptId == attemptId && a.QuestionId == questionId);
        }

        public async Task<bool> AnswerExistsAsync(int attemptId, int questionId)
        {
            return await _context.CompanyExamQuestionAnswers
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

        public async Task<IEnumerable<CompanyExamQuestionAnswer>> GetTextAnswersAsync(int attemptId)
        {
            return await _context.CompanyExamQuestionAnswers
                .Where(a => a.AttemptId == attemptId && !string.IsNullOrEmpty(a.TextAnswer))
                .OrderBy(a => a.QuestionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyExamQuestionAnswer>> GetMultipleChoiceAnswersAsync(int attemptId)
        {
            return await _context.CompanyExamQuestionAnswers
                .Where(a => a.AttemptId == attemptId && !string.IsNullOrEmpty(a.SelectedOptionsJson))
                .OrderBy(a => a.QuestionId)
                .ToListAsync();
        }
    }
}