namespace Fit4Job.Repositories.Implementations
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.DeletedAt == null);
        }

        public async Task<ApplicationUser?> GetByUserNameAsync(string userName)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName && u.DeletedAt == null);
        }

        public async Task<ApplicationUser?> GetUserWithProfileAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.AdminProfile)
                .Include(u => u.CompanyProfile)
                .Include(u => u.JobSeekerProfile)
                .FirstOrDefaultAsync(u => u.Id == userId && u.DeletedAt == null);
        }

        public async Task<bool> SoftDeleteAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null && user.DeletedAt == null)
            {
                user.DeletedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;
                Update(user);
                return true;
            }
            return false;
        }

        public async Task<bool> RestoreAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.DeletedAt != null);

            if (user != null)
            {
                user.DeletedAt = null;
                user.UpdatedAt = DateTime.UtcNow;
                Update(user);
                return true;
            }
            return false;
        }

        // Override the base GetByIdAsync to exclude soft-deleted users
        public override async Task<ApplicationUser?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);
        }

        // Override GetAllAsync to exclude soft-deleted users
        public override async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => u.DeletedAt == null)
                .ToListAsync();
        }
    }
}