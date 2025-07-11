namespace Fit4Job.Repositories.Implementations
{
    public class TrackQuestionAnswerRepository : GenericRepository<TrackQuestionAnswer>, ITrackQuestionAnswerRepository
    {
        public TrackQuestionAnswerRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<TrackQuestionAnswer>> GetAllAnswersByAttemptAsync(int attemptId)
        {
            return await _context.TrackQuestionAnswers
                .Where(a => a.AttemptId == attemptId)
                .ToListAsync();
        }

        public async Task<int> CountCorrectAnswersInAttemptAsync(int attemptId)
        {
            return await _context.TrackQuestionAnswers
                .Where(a => a.AttemptId == attemptId && a.IsCorrect)
                .CountAsync();
        }

        public async Task<TrackQuestionAnswer?> GetAnswerForQuestionInAttemptAsync(int attemptId, int questionId)
        {
            return await _context.TrackQuestionAnswers
                .FirstOrDefaultAsync(a => a.AttemptId == attemptId && a.QuestionId == questionId);
        }

        public async Task<IEnumerable<TrackQuestionAnswer>> GetTextAnswersOnlyByAttemptAsync(int attemptId)
        {
            return await _context.TrackQuestionAnswers
                .Where(a => a.AttemptId == attemptId && !string.IsNullOrEmpty(a.TextAnswer))
                .ToListAsync();
        }
    }
}
