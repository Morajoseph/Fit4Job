namespace Fit4Job.Repositories.Implementations
{
    public class CompanyProfileRepository : GenericRepository<CompanyProfile>, ICompanyProfileRepository
    {
        public CompanyProfileRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public override async Task<CompanyProfile?> GetByIdAsync(int id)
        {
            return await _context.CompanyProfiles
                .Include(cp => cp.User)
                .Include(cp => cp.Jobs)
                .FirstOrDefaultAsync(cp => cp.Id == id);
        }
        public override async Task<IEnumerable<CompanyProfile>> GetAllAsync()
        {
            return await _context.CompanyProfiles
                .Include(cp => cp.User)
                .Include(cp => cp.Jobs)
                .OrderBy(cp => cp.CompanyName)
                .ToListAsync();
        }

        public async Task<CompanyProfile?> GetByUserIdAsync(int userId)
        {
            return await _context.CompanyProfiles
                .Include(cp => cp.User)
                .FirstOrDefaultAsync(ap => ap.UserId == userId);
        }

        public async Task<CompanyProfile?> GetCompanyByUserIdAsync(int userId)
        {
            return await _context.CompanyProfiles
                .Include(cp => cp.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<IEnumerable<CompanyProfile>> GetCompaniesByStatusAsync(CompanyStatus status)
        {
            return await _context.CompanyProfiles
                .Where(c => c.Status == status)
                .OrderBy(c => c.CompanyName)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyProfile>> SearchCompaniesByNameAsync(string name)
        {
            return await _context.CompanyProfiles
                .Where(c => c.CompanyName.Contains(name))
                .OrderBy(c => c.CompanyName)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyProfile>> GetCompaniesByIndustryAsync(string industry)
        {
            return await _context.CompanyProfiles
                .Where(c => c.Industry == industry)
                .OrderBy(c => c.CompanyName)
                .ToListAsync();
        }
        public async Task<IEnumerable<CompanyProfile>> GetPendingCompaniesAsync()
        {
            return await _context.CompanyProfiles
                .Where(c => c.Status == CompanyStatus.Pending && c.DeletedAt == null)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
