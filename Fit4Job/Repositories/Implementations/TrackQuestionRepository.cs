namespace Fit4Job.Repositories.Implementations
{
    public class TrackQuestionRepository : GenericRepository<TrackQuestion>, ITrackQuestionRepository
    {
        public TrackQuestionRepository(Fit4JobDbContext context) : base(context)
        {

        }
        public async Task<IEnumerable<TrackQuestion>> GetActiveByTrackIdAsync(int trackId)
        {
            return await _context.TrackQuestions
                .Where(q => q.TrackId == trackId && q.IsActive)
                .ToListAsync();
        }
        public async Task<IEnumerable<TrackQuestion>> GetQuestionsByTrackIdAsync(int trackId)
        {
            return await _context.TrackQuestions
                .Where(q => q.TrackId == trackId)
                .OrderBy(q => q.Id)
                .ToListAsync();
        }

        public async Task<int> CountQuestionsInTrackAsync(int trackId)
        {
            return await _context.TrackQuestions
                .Where(q => q.TrackId == trackId && q.IsActive)
                .CountAsync();
        }

        public async Task<decimal> GetTotalPointsForTrackAsync(int trackId)
        {
            return await _context.TrackQuestions
                .Where(q => q.TrackId == trackId && q.IsActive)
                .SumAsync(q => q.Points);
        }

        public async Task<TrackQuestion?> GetNextQuestionInTrackAsync(int trackId, int lastQuestionId)
        {
            return await _context.TrackQuestions
                .Where(q => q.TrackId == trackId && q.Id > lastQuestionId && q.IsActive)
                .OrderBy(q => q.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<TrackQuestion?> GetPreviousQuestionInTrackAsync(int trackId, int currentQuestionId)
        {
            return await _context.TrackQuestions
                .Where(q => q.TrackId == trackId && q.Id < currentQuestionId && q.IsActive)
                .OrderByDescending(q => q.Id)
                .FirstOrDefaultAsync();
        }
    }
}
