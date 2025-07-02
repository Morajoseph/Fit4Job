using Fit4Job.Repositories.Generic;
using Fit4Job.Repositories.Interfaces;

namespace Fit4Job.Repositories.Implementations
{
    public class TrackAttemptRepository : GenericRepository<TrackAttempt>, ITrackAttemptRepository
    {
        public TrackAttemptRepository(Fit4JobDbContext context) : base(context)
        {
        }
    }
}
