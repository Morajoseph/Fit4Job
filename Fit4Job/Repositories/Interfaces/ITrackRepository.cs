namespace Fit4Job.Repositories.Interfaces
{
    public interface ITrackRepository : IGenericRepository<Track>
    {
        Task<IEnumerable<Track>> GetAllTracksByCategoryIdWithQuestionsAsync(int categoryId);
        Task<IEnumerable<Track>> GetActiveTracksAsync();
        Task<IEnumerable<Track>> GetAllTracksIncludingDeletedAsync();
        Task<IEnumerable<Track>> SearchTracksAsync(TrackSearchDTO searchDTO);
        Task<Track?> GetTrackWithDetailsAsync(int id);
    }
}
