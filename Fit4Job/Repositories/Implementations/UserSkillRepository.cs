using Fit4Job.Repositories.Generic;
using Fit4Job.Repositories.Interfaces;

namespace Fit4Job.Repositories.Implementations
{
    public class UserSkillRepository : GenericRepository<UserSkill>, IUserSkilRepository
    {
        public UserSkillRepository(Fit4JobDbContext context) : base(context)
        {
        }
    }
}
