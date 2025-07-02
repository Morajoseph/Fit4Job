using Fit4Job.Repositories.Generic;
using Fit4Job.Repositories.Interfaces;

namespace Fit4Job.Repositories.Implementations
{
    public class UserBadgeRepository : GenericRepository<UserBadge>, IUserBadgeRepository
    {
        public UserBadgeRepository(Fit4JobDbContext context) : base(context)
        {
        }
    }
}
