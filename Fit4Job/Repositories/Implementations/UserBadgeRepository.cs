namespace Fit4Job.Repositories.Implementations
{
    public class UserBadgeRepository : GenericRepository<UserBadge>, IUserBadgeRepository
    {
        public UserBadgeRepository(Fit4JobDbContext context) : base(context)
        {

        }


        public async Task<IEnumerable<UserBadge>> GetBadgesByUserIdAsync(int userId)
        {
            return await _context.UserBadges
                .Include(ub => ub.Badge)
                .Include(ub => ub.TrackAttempt)
                .Where(ub => ub.UserId == userId)
                .OrderByDescending(ub => ub.EarnedAt)
                .ToListAsync();
        }

        public async Task<bool> HasUserEarnedBadgeAsync(int userId, int badgeId)
        {
            return await _context.UserBadges
                .AnyAsync(ub => ub.UserId == userId && ub.BadgeId == badgeId);
        }

        public async Task<IEnumerable<UserBadge>> GetBadgesByAttemptIdAsync(int attemptId)
        {
            return await _context.UserBadges
                .Where(ub => ub.TrackAttemptId == attemptId)
                .ToListAsync();
        }

        public async Task<int> CountUserBadgesAsync(int userId)
        {
            return await _context.UserBadges
                .CountAsync(ub => ub.UserId == userId);
        }

    }
}
