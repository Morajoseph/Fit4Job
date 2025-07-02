namespace Fit4Job.Repositories.Interfaces
{
    public interface ISkillRepository : IGenericRepository<Skill>
    {
        Task<Skill?> GetSkillByNameAsync(string skillName);
        Task<IEnumerable<Skill>> GetAllActiveSkillsAsync();
    }
}
