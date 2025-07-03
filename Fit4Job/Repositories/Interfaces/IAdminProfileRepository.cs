namespace Fit4Job.Repositories.Interfaces
{
    public interface IAdminProfileRepository : IGenericRepository<AdminProfile>
    {
        // Admin-specific operations
        Task<AdminProfile?> GetByUserIdAsync(int userId);
        Task<AdminProfile?> GetAdminWithUserDetailsAsync(int adminId);
        Task<AdminProfile?> GetAdminWithUserDetailsByUserIdAsync(int userId);
        Task<bool> ExistsByUserIdAsync(int userId);
        Task<bool> IsActiveAdminAsync(int userId);
        Task SoftDeleteAsync(int adminId);
        Task RestoreAsync(int adminId);
    }
}
