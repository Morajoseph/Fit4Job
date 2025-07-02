namespace Fit4Job.Repositories.Implementations
{
    public class TrackQuestionAnswerRepository : GenericRepository<TrackQuestionAnswer>, ITrackQuestionAnswerRepository
    {
        public TrackQuestionAnswerRepository(Fit4JobDbContext context) : base(context)
        {

        }
    }
}
