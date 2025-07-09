namespace Fit4Job.Repositories.Interfaces
{
    public interface ITrackQuestionOptionRepository : IGenericRepository<TrackQuestionOption>
    {

        
        Task<IEnumerable<TrackQuestionOption>> GetOptionsByQuestionIdAsync(int questionId);
        Task<IEnumerable<TrackQuestionOption>> GetCorrectOptionsAsync(int questionId);
        Task<IEnumerable<TrackQuestionOption>> GetIncorrectOptionsAsync(int questionId);
        Task<int> CountCorrectOptionsAsync(int questionId);
        Task<bool> HasMultipleCorrectOptionsAsync(int questionId);
        
       
        //if single choise
        Task<bool> HasExactlyOneCorrectOptionAsync(int questionId);
    
    }
}
