namespace Fit4Job.Repositories.Implementations
{
    public class TrackQuestionOptionRepository : GenericRepository<TrackQuestionOption>, ITrackQuestionOptionRepository
    {
        public TrackQuestionOptionRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<TrackQuestionOption>> GetOptionsByQuestionIdAsync(int questionId)
        {
            return await _context.TrackQuestionOptions
                .Where(o => o.QuestionId == questionId)
                .ToListAsync();
        }
        public async Task<IEnumerable<TrackQuestionOption>> GetActiveOptionsByQuestionIdAsync(int questionId)
        {
            return await _context.TrackQuestionOptions
                .Where(o => o.QuestionId == questionId && o.DeletedAt == null)
                .ToListAsync();
        }
        public async Task<IEnumerable<TrackQuestionOption>> GetCorrectOptionsAsync(int questionId)
        {
            return await _context.TrackQuestionOptions
                .Where(o => o.QuestionId == questionId && o.IsCorrect && o.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<TrackQuestionOption>> GetIncorrectOptionsAsync(int questionId)
        {
            return await _context.TrackQuestionOptions
                .Where(o => o.QuestionId == questionId && !o.IsCorrect && o.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<int> CountCorrectOptionsAsync(int questionId)
        {
            return await _context.TrackQuestionOptions
                .Where(o => o.QuestionId == questionId && o.IsCorrect && o.DeletedAt == null)
                .CountAsync();
        }

        public async Task<bool> HasMultipleCorrectOptionsAsync(int questionId)
        {
            return await _context.TrackQuestionOptions
                .Where(o => o.QuestionId == questionId && o.IsCorrect && o.DeletedAt == null)
                .CountAsync() > 1;
        }

        public async Task<bool> HasExactlyOneCorrectOptionAsync(int questionId)
        {
            return await _context.TrackQuestionOptions
                .CountAsync(o => o.QuestionId == questionId && o.IsCorrect && o.DeletedAt == null) == 1;
        }


    }
}
