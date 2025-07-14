namespace Fit4Job.Repositories.Interfaces
{
    public interface IUserSkillRepository : IGenericRepository<UserSkill>
    {
        Task<IEnumerable<UserSkill>> GetSkillsByUserIdAsync(int userId);

        Task<UserSkill?> GetActiveUserSkillAsync(int userId, int skillId);

        Task<IEnumerable<Skill>> GetActiveSkillsByUserIdAsync(int userId);

        Task<UserSkill?> GetDeletedUserSkillAsync(int userId, int skillId);



        //Task<bool> HasAnySkillFromListAsync(int userId, List<int> skillIds);

    }
}
