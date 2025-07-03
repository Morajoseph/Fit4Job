namespace Fit4Job.Repositories.Interfaces
{
    public interface IUserSkillRepository : IGenericRepository<UserSkill>
    {
        Task<IEnumerable<UserSkill>> GetSkillsByUserIdAsync(int userId);
        //Task<bool> HasAnySkillFromListAsync(int userId, List<int> skillIds);

    }
}
