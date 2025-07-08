namespace Fit4Job.Repositories.Implementations
{
    public class TrackRepository : GenericRepository<Track>, ITrackRepository
    {
        public TrackRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Track>> GetActiveTracksAsync()
        {
            return await _context.Tracks
                  .Where(t => t.IsActive)
                  .ToListAsync();
        }

        public async Task<IEnumerable<Track>> GetAllTracksByCategoryAsync(string categoryName)
        {
            return await _context.Tracks
                .Where(t => t.Category.Name == categoryName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Track>> GetAllTracksByCategoryIdAsync(int categoryId)
        {
            return await _context.Tracks
                .Where(t => t.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Track>> GetPremiumTracksAsync()
        {
            return await _context.Tracks
                .Where(t => t.IsPremium)
                .ToListAsync();
        }

        public async Task<Track> GetTrackByNameAsync(string trackName)
        {
            return await _context.Tracks
                .FirstOrDefaultAsync(t => t.Name == trackName);
        }

        public async Task<Track> GetTrackWithQuestionsAsync(int trackId)
        {
            return await _context.Tracks
                .Include(t => t.TrackQuestions)
                .FirstOrDefaultAsync(t => t.Id == trackId);
        }

        //
        public async Task<IEnumerable<Badge>> GetBadgesByTrackIdAsync(int trackId)
        {
            return await _context.Badges
                .Where(b => b.TrackId == trackId)
                .ToListAsync();
        }


        public async Task<Track?> GetTrackWithDetailsAsync(int id)
        {
            return await _context.Tracks
                .Include(t => t.Category)
                .Include(t => t.Creator)
                .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);
        }

    }
}
