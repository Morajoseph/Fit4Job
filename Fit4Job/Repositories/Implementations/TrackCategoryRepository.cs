namespace Fit4Job.Repositories.Implementations
{
    public class TrackCategoryRepository : GenericRepository<TrackCategory>, ITrackCategoryRepository
    {
        public TrackCategoryRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<TrackCategory>> SearchByNameAsync(string keyword)
        {
            return await _context.TrackCategories
                .Where(tc => tc.Name.Contains(keyword) && tc.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<TrackCategory>> GetActiveCategoriesAsync()
        {
            return await _context.TrackCategories
                .Where(tc => tc.DeletedAt == null)
                .ToListAsync();
        }

        public override async Task<IEnumerable<TrackCategory>> GetAllAsync()
        {
            return await _context.TrackCategories
                .Where(b => b.DeletedAt == null)
                .ToListAsync();
        }

    }
}
