namespace Fit4Job.Repositories.Implementations
{
    public class SkillRepository : GenericRepository<Skill>, ISkillRepository
    {
        public SkillRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Skill>> GetAllActiveSkillsAsync()
        {
            return await _context.Skills
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<Skill?> GetSkillByNameAsync(string skillName)
        {
            return await _context.Skills
                .FirstOrDefaultAsync(s => s.Name == skillName);
        }
    }
}
