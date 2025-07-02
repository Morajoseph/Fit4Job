using Fit4Job.Repositories.Generic;
using Fit4Job.Repositories.Interfaces;

namespace Fit4Job.Repositories.Implementations
{
    public class TrackQuestionAnswerRepository : GenericRepository<TrackQuestionAnswer>, ITrackQuestionAnswerRepository
    {
        public TrackQuestionAnswerRepository(Fit4JobDbContext context) : base(context)
        {
        }
    }
}
