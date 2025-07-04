namespace Fit4Job.Repositories.Interfaces
{
    public interface IBadgeRepository : IGenericRepository<Badge>
    {
        Task<IEnumerable<Badge>> GetByTrackIdAsync(int trackId);
        Task<bool> SoftDeleteAsync(int badgeId);
        Task<bool> RestoreAsync(int badgeId);
        Task<bool> SoftDeleteByTrackAsync(int trackId);
    }
}
