namespace Fit4Job.Repositories.Interfaces
{
    public interface IAdminProfileRepository : IGenericRepository<AdminProfile>
    {
        Task<AdminProfile?> GetByUserIdAsync(int userId);
        Task<AdminProfile?> GetAdminWithUserDetailsByUserIdAsync(int userId);
        Task<bool> IsActiveAdminAsync(int userId);
        Task SoftDeleteAsync(int adminId);
        Task RestoreAsync(int adminId);
    }
}
