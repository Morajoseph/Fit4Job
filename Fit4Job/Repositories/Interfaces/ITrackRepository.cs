namespace Fit4Job.Repositories.Interfaces
{
    public interface ITrackRepository : IGenericRepository<Track>
    {
        Task<Track> GetTrackByNameAsync(string trackName);
        Task<IEnumerable<Track>> GetAllTracksByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Track>> GetAllTracksByCategoryAsync(string categoryName);
        Task<IEnumerable<Track>> GetPremiumTracksAsync();
        Task<IEnumerable<Track>> GetActiveTracksAsync();
        Task<Track> GetTrackWithQuestionsAsync(int trackId);

        //
        Task<IEnumerable<Badge>> GetBadgesByTrackIdAsync(int trackId);
        Task<Track?> GetTrackWithDetailsAsync(int id);

    }
}
