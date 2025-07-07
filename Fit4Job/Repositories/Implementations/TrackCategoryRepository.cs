namespace Fit4Job.Repositories.Implementations
{
    public class TrackCategoryRepository : GenericRepository<TrackCategory>, ITrackCategoryRepository
    {
        public TrackCategoryRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<TrackCategory>> SearchByNameAndStatusAsync(string keyword, bool isActive)
        {
            return await _context.TrackCategories
                .Where(tc => tc.Name.Contains(keyword) && (isActive ? tc.DeletedAt == null : tc.DeletedAt != null))
                .ToListAsync();
        }

        public async Task<IEnumerable<TrackCategory>> GetActiveCategoriesAsync()
        {
            return await _context.TrackCategories
                .Where(tc => tc.DeletedAt == null)
                .ToListAsync();
        }
    }
}
