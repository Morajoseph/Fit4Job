namespace Fit4Job.Repositories.Interfaces
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<ApplicationUser?> GetByUserNameAsync(string userName);
        Task<ApplicationUser?> GetUserWithProfileAsync(int userId);
        Task<bool> SoftDeleteAsync(int userId);
        Task<bool> RestoreAsync(int userId);
    }
}