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
                  .Where(t => t.DeletedAt == null)
                  .AsNoTracking()
                  .ToListAsync();
        }

        public async Task<IEnumerable<Track>> GetAllTracksByCategoryIdWithQuestionsAsync(int categoryId)
        {
            return await _context.Tracks
                .Where(t => t.CategoryId == categoryId && t.DeletedAt == null)
                .Include(t => t.TrackQuestions)
                .ToListAsync();
        }

        public async Task<IEnumerable<Track>> GetAllTracksIncludingDeletedAsync()
        {
            return await _context.Tracks
                .ToListAsync();
        }

        public async Task<IEnumerable<Track>> SearchTracksAsync(TrackSearchDTO dto)
        {
            IQueryable<Track> query = _context.Tracks.AsQueryable();

            if (!string.IsNullOrEmpty(dto.Name))
                query = query.Where(t => t.Name.Contains(dto.Name));

            if (dto.CategoryId.HasValue)
                query = query.Where(t => t.CategoryId == dto.CategoryId.Value);

            if (dto.CreatorId.HasValue)
                query = query.Where(t => t.CreatorId == dto.CreatorId.Value);

            if (dto.IsActive.HasValue)
                query = query.Where(t => dto.IsActive.Value ? t.DeletedAt == null : t.DeletedAt != null);

            if (dto.MinPrice.HasValue)
                query = query.Where(t => t.Price >= dto.MinPrice.Value);

            if (dto.MaxPrice.HasValue)
                query = query.Where(t => t.Price <= dto.MaxPrice.Value);

            return await query.ToListAsync();
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
