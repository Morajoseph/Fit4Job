namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyProfileRepository : IGenericRepository<CompanyProfile>
    {
        Task<CompanyProfile?> GetByUserIdAsync(int userId);
        Task<CompanyProfile?> GetCompanyByUserIdAsync(int userId);
        Task<IEnumerable<CompanyProfile>> GetCompaniesByStatusAsync(CompanyStatus status);
        Task<IEnumerable<CompanyProfile>> SearchCompaniesByNameAsync(string name);
        Task<IEnumerable<CompanyProfile>> GetCompaniesByIndustryAsync(string industry);
    }
}