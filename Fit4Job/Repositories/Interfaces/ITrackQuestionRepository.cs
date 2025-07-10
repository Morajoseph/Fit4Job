namespace Fit4Job.Repositories.Interfaces
{
    public interface ITrackQuestionRepository : IGenericRepository<TrackQuestion>
    {
        Task<IEnumerable<TrackQuestion>> GetActiveByTrackIdAsync(int trackId);
        Task<IEnumerable<TrackQuestion>> GetQuestionsByTrackIdAsync(int trackId);
        Task<int> CountQuestionsInTrackAsync(int trackId);
        Task<decimal> GetTotalPointsForTrackAsync(int trackId);
        Task<TrackQuestion?> GetNextQuestionInTrackAsync(int trackId, int lastQuestionId);
        Task<TrackQuestion?> GetPreviousQuestionInTrackAsync(int trackId, int currentQuestionId);
    }
}
