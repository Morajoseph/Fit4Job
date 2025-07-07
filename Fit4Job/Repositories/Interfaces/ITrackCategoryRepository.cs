namespace Fit4Job.Repositories.Interfaces
{
    public interface ITrackCategoryRepository : IGenericRepository<TrackCategory>
    {
        
        
        Task<IEnumerable<TrackCategory>> SearchByNameAndStatusAsync(string keyword, bool isActive);
        Task<IEnumerable<TrackCategory>> GetActiveCategoriesAsync();


    }
}
