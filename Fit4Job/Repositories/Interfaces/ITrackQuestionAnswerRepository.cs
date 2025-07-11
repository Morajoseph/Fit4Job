namespace Fit4Job.Repositories.Interfaces
{
    public interface ITrackQuestionAnswerRepository : IGenericRepository<TrackQuestionAnswer>
    {
        Task<IEnumerable<TrackQuestionAnswer>> GetAllAnswersByAttemptAsync(int attemptId);
        Task<int> CountCorrectAnswersInAttemptAsync(int attemptId);
        Task<TrackQuestionAnswer?> GetAnswerForQuestionInAttemptAsync(int attemptId, int questionId);
        Task<IEnumerable<TrackQuestionAnswer>> GetTextAnswersOnlyByAttemptAsync(int attemptId);
    }
}
 