namespace Fit4Job.Repositories.Implementations
{
    public class AdminProfileRepository : GenericRepository<AdminProfile>, IAdminProfileRepository
    {
        public AdminProfileRepository(Fit4JobDbContext context) : base(context)
        {

        }
        public async Task<AdminProfile?> GetByUserIdAsync(int userId)
        {
            return await _context.Set<AdminProfile>()
                .FirstOrDefaultAsync(ap => ap.UserId == userId);
        }

        public async Task<IEnumerable<AdminProfile>> GetActiveAdminsAsync()
        {
            return await _context.Set<AdminProfile>()
                .Where(ap => ap.DeletedAt == null)
                .OrderBy(ap => ap.FirstName)
                .ThenBy(ap => ap.LastName)
                .ToListAsync();
        }

        public async Task<AdminProfile?> GetAdminWithUserDetailsAsync(int adminId)
        {
            return await _context.Set<AdminProfile>()
                .Include(ap => ap.User)
                .FirstOrDefaultAsync(ap => ap.Id == adminId);
        }

        public async Task<AdminProfile?> GetAdminWithUserDetailsByUserIdAsync(int userId)
        {
            return await _context.Set<AdminProfile>()
                .Include(ap => ap.User)
                .FirstOrDefaultAsync(ap => ap.UserId == userId);
        }

        public async Task<bool> ExistsByUserIdAsync(int userId)
        {
            return await _context.Set<AdminProfile>()
                .AnyAsync(ap => ap.UserId == userId);
        }

        public async Task<bool> IsActiveAdminAsync(int userId)
        {
            return await _context.Set<AdminProfile>()
                .AnyAsync(ap => ap.UserId == userId && ap.DeletedAt == null);
        }

        public async Task SoftDeleteAsync(int adminId)
        {
            var admin = await GetByIdAsync(adminId);
            if (admin != null && admin.DeletedAt == null)
            {
                admin.DeletedAt = DateTime.UtcNow;
                admin.UpdatedAt = DateTime.UtcNow;
                Update(admin);
            }
        }

        public async Task RestoreAsync(int adminId)
        {
            var admin = await GetByIdAsync(adminId);
            if (admin != null && admin.DeletedAt != null)
            {
                admin.DeletedAt = null;
                admin.UpdatedAt = DateTime.UtcNow;
                Update(admin);
            }
        }
    }
}
