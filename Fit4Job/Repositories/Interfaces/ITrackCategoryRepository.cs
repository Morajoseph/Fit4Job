namespace Fit4Job.Repositories.Interfaces
{
    public interface ITrackCategoryRepository : IGenericRepository<TrackCategory>
    {

        Task<IEnumerable<TrackCategory>> SearchByNameAsync(string keyword);

        Task<IEnumerable<TrackCategory>> GetActiveCategoriesAsync();


    }
}
