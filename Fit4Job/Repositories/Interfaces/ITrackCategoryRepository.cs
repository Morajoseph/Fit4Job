namespace Fit4Job.Repositories.Interfaces
{
    public interface ITrackCategoryRepository : IGenericRepository<TrackCategory>
    {
        Task<IEnumerable<TrackCategory>> GetActiveCategoriesAsync();
        Task<IEnumerable<TrackCategory>> SearchByNameAsync(string keyword);
    }
}
