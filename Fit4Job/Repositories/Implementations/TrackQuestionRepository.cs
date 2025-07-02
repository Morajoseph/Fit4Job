using Fit4Job.Repositories.Generic;
using Fit4Job.Repositories.Interfaces;

namespace Fit4Job.Repositories.Implementations
{
    public class TrackQuestionRepository : GenericRepository<TrackQuestion>, ITrackQuestionRepository
    {
        public TrackQuestionRepository(Fit4JobDbContext context) : base(context)
        {
        }
    }
}
