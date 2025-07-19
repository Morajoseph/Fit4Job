namespace Fit4Job.Repositories.Implementations
{
    public class AdminProfileRepository : GenericRepository<AdminProfile>, IAdminProfileRepository
    {
        public AdminProfileRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public override async Task<AdminProfile?> GetByIdAsync(int id)
        {
            return await _context.AdminProfiles
                .Include(ap => ap.User)
                .FirstOrDefaultAsync(ap => ap.Id == id);
        }

        public override async Task<IEnumerable<AdminProfile>> GetAllAsync()
        {
            return await _context.AdminProfiles
                .Include(ap => ap.User)
                .OrderBy(ap => ap.User.Email)
                .ToListAsync();
        }
        public async Task<AdminProfile?> GetByUserIdAsync(int userId)
        {
            return await _context.AdminProfiles
                .FirstOrDefaultAsync(ap => ap.UserId == userId);
        }

        public async Task<AdminProfile?> GetAdminWithUserDetailsByUserIdAsync(int userId)
        {
            return await _context.AdminProfiles
                .Include(ap => ap.User)
                .FirstOrDefaultAsync(ap => ap.UserId == userId);
        }

        public async Task<bool> IsActiveAdminAsync(int userId)
        {
            return await _context.AdminProfiles
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
