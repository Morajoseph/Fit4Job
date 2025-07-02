namespace Fit4Job.Repositories.Implementations
{
    public class UserSkillRepository : GenericRepository<UserSkill>, IUserSkilRepository
    {
        public UserSkillRepository(Fit4JobDbContext context) : base(context)
        {

        }
    }
}
