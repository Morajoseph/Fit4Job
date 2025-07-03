namespace Fit4Job.Repositories.Interfaces
{
    public interface IUserBadgeRepository : IGenericRepository<UserBadge>
    {
        Task<IEnumerable<UserBadge>> GetBadgesByUserIdAsync(int userId);
        Task<bool> HasUserEarnedBadgeAsync(int userId, int badgeId);
        Task<IEnumerable<UserBadge>> GetBadgesByAttemptIdAsync(int attemptId);
        Task<int> CountUserBadgesAsync(int userId);




    }
}
