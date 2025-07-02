namespace Fit4Job.Repositories.Implementations
{
    public class TrackCategoryRepository : GenericRepository<TrackCategory>, ITrackCategoryRepository
    {
        public TrackCategoryRepository(Fit4JobDbContext context) : base(context)
        {

        }
    }
}
