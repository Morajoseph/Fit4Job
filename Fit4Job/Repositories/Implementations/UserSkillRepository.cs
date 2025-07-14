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
                .Where(us => us.UserId == userId )
                .ToListAsync();
        }

        public async Task<UserSkill?> GetActiveUserSkillAsync(int userId, int skillId)
        {
            return await _context.UserSkills
                .FirstOrDefaultAsync(us =>
                    us.UserId == userId &&
                    us.SkillId == skillId &&
                    us.DeletedAt == null);
        }

        public async Task<IEnumerable<Skill>> GetActiveSkillsByUserIdAsync(int userId)
        {
            return await _context.UserSkills
                .Where(us => us.UserId == userId && us.DeletedAt == null)
                .Include(us => us.Skill)
                .Select(us => us.Skill)
                .ToListAsync();
        }

        public async Task<UserSkill?> GetDeletedUserSkillAsync(int userId, int skillId)
        {
            return await _context.UserSkills
                .FirstOrDefaultAsync(us =>
                    us.UserId == userId &&
                    us.SkillId == skillId &&
                    us.DeletedAt != null);
        }


    }
}
