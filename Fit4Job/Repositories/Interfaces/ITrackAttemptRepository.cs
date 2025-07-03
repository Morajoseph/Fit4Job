namespace Fit4Job.Repositories.Interfaces
{
    public interface ITrackAttemptRepository : IGenericRepository<TrackAttempt>
    {
        Task<IEnumerable<TrackAttempt>> GetAllAttemptsByUserAsync(int userId);
       // Task<TrackAttempt?> GetFirstAttemptAsync(int userId, int trackId);

        Task<int> CountCompletedAttemptsAsync(int userId, int trackId);

        Task<decimal> GetTotalScoreByUserInTrackAsync(int userId, int trackId);

        Task<IEnumerable<TrackAttempt>> GetAllAttemptsByUserInTrackAsync(int userId, int trackId);



    }
}
