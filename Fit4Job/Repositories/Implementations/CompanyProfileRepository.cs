

namespace Fit4Job.Repositories.Implementations
{
    public class CompanyProfileRepository : GenericRepository<CompanyProfile>, ICompanyProfileRepository
    {
        public CompanyProfileRepository(Fit4JobDbContext context) : base(context)
        {
        }

        public async Task<CompanyProfile?> GetCompanyByUserIdAsync(int userId)
        {
            return await _context.CompanyProfiles.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<IEnumerable<CompanyProfile>> GetCompaniesByStatusAsync(CompanyStatus status)
        {
            return await _context.CompanyProfiles.Where(c => c.Status == status)
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
    
    }
}
