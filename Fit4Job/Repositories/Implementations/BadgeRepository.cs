namespace Fit4Job.Repositories.Implementations
{
    public class BadgeRepository : GenericRepository<Badge>, IBadgeRepository
    {
        public BadgeRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Badge>> GetByTrackIdAsync(int trackId)
        {
            return await _context.Badges
                .Where(b => b.TrackId == trackId && b.DeletedAt == null)
                .OrderBy(b => b.PointsRequired)
                .ToListAsync();
        }

        public async Task<Badge?> GetByNameAsync(string name)
        {
            return await _context.Badges
                .FirstOrDefaultAsync(b => b.Name == name && b.DeletedAt == null);
        }

        public async Task<Badge?> GetBadgeWithTrackAsync(int badgeId)
        {
            return await _context.Badges
                .Include(b => b.Track)
                .FirstOrDefaultAsync(b => b.Id == badgeId && b.DeletedAt == null);
        }

        public async Task<IEnumerable<Badge>> GetBadgesByTrackNameAsync(string trackName)
        {
            return await _context.Badges
                .Include(b => b.Track)
                .Where(b => b.Track.Name == trackName && b.DeletedAt == null)
                .OrderBy(b => b.PointsRequired)
                .ToListAsync();
        }

        public async Task<bool> SoftDeleteAsync(int badgeId)
        {
            var badge = await GetByIdAsync(badgeId);
            if (badge != null && badge.DeletedAt == null)
            {
                badge.DeletedAt = DateTime.UtcNow;
                badge.UpdatedAt = DateTime.UtcNow;
                Update(badge);
                return true;
            }
            return false;
        }

        public async Task<bool> RestoreAsync(int badgeId)
        {
            var badge = await _context.Badges
                .FirstOrDefaultAsync(b => b.Id == badgeId && b.DeletedAt != null);

            if (badge != null)
            {
                badge.DeletedAt = null;
                badge.UpdatedAt = DateTime.UtcNow;
                Update(badge);
                return true;
            }
            return false;
        }

        public async Task<bool> SoftDeleteByTrackAsync(int trackId)
        {
            var badges = await _context.Badges
                .Where(b => b.TrackId == trackId && b.DeletedAt == null)
                .ToListAsync();

            if (badges.Any())
            {
                var now = DateTime.UtcNow;
                foreach (var badge in badges)
                {
                    badge.DeletedAt = now;
                    badge.UpdatedAt = now;
                }
                _context.Badges.UpdateRange(badges);
                return true;
            }
            return false;
        }

        public override async Task<Badge?> GetByIdAsync(int id)
        {
            return await _context.Badges
                .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null);
        }

        public override async Task<IEnumerable<Badge>> GetAllAsync()
        {
            return await _context.Badges
                .Where(b => b.DeletedAt == null)
                .OrderBy(b => b.Name)
                .ToListAsync();
        }
    }
}