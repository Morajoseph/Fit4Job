namespace Fit4Job.Repositories.Implementations
{
    public class TrackQuestionOptionRepository : GenericRepository<TrackQuestionOption>, ITrackQuestionOptionRepository
    {
        public TrackQuestionOptionRepository(Fit4JobDbContext context) : base(context)
        {

        }
    }
}
