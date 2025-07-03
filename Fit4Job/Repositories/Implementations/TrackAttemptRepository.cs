namespace Fit4Job.Repositories.Implementations
{
    public class TrackAttemptRepository : GenericRepository<TrackAttempt>, ITrackAttemptRepository
    {
        public TrackAttemptRepository(Fit4JobDbContext context) : base(context)
        {

        }
        public async Task<IEnumerable<TrackAttempt>> GetAllAttemptsByUserAsync(int userId)
        {
            return await _context.TrackAttempts
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        //public async Task<TrackAttempt?>GetFirstAttemptAsync(int userId, int trackId)
        //{
        //    return await _context.TrackAttempts
        //        .Where(a => a.UserId == userId && a.TrackId == trackId)
        //        .OrderBy(a => a.StartTime)
        //        .FirstOrDefaultAsync();
        //}

        public async Task<int> CountCompletedAttemptsAsync(int userId, int trackId)
        {
            return await _context.TrackAttempts
                .Where(a => a.UserId == userId && a.TrackId == trackId && a.Status == AttemptStatus.Completed)
                .CountAsync();
        }

        public async Task<decimal> GetTotalScoreByUserInTrackAsync(int userId, int trackId)
        {
            return await _context.TrackAttempts
                .Where(a => a.UserId == userId && a.TrackId == trackId && a.Status == AttemptStatus.Completed)
                .SumAsync(a => a.TotalScore);
        }

        public async Task<IEnumerable<TrackAttempt>> GetAllAttemptsByUserInTrackAsync(int userId, int trackId)
        {
            return await _context.TrackAttempts
                .Where(a => a.UserId == userId && a.TrackId == trackId)
               // .OrderByDescending(a => a.StartTime) 
                .ToListAsync();
        }

    }
}
