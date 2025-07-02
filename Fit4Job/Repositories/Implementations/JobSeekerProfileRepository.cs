using Fit4Job.Repositories.Generic;
using Fit4Job.Repositories.Interfaces;

namespace Fit4Job.Repositories.Implementations
{
    public class JobSeekerProfileRepository : GenericRepository<JobSeekerProfile>, IJobSeekerProfileRepository
    {
        public JobSeekerProfileRepository(Fit4JobDbContext context) : base(context)
        {
        }
    }
}
