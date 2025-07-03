namespace Fit4Job.Repositories.Implementations
{
    public class UserSkillRepository : GenericRepository<UserSkill>, IUserSkillRepository
    {
        public UserSkillRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<UserSkill>> GetSkillsByUserIdAsync(int userId)
        {
            return await _context.UserSkills
                .Include(us => us.Skill)  
                .Where(us => us.UserId == userId && us.DeletedAt == null)
                .ToListAsync();
        }
    }
}
