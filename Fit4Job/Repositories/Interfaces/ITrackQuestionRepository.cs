namespace Fit4Job.Repositories.Interfaces
{
    public interface ITrackQuestionRepository : IGenericRepository<TrackQuestion>
    {

        Task<IEnumerable<TrackQuestion>> GetQuestionsByTrackIdAsync(int trackId);
        Task<int> CountQuestionsInTrackAsync(int trackId);
        Task<decimal> GetTotalPointsForTrackAsync(int trackId);
       // Task<IEnumerable<TrackQuestion>> GetQuestionsByDifficultyAsync(string difficultyLevel);
       // Task<IEnumerable<TrackQuestion>> GetQuestionsByLevelAsync(string questionLevel);
        Task<TrackQuestion?> GetNextQuestionInTrackAsync(int trackId, int lastQuestionId);
        Task<TrackQuestion?> GetPreviousQuestionInTrackAsync(int trackId, int currentQuestionId);

    }
}
