namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyTaskRepository : IGenericRepository<CompanyTask>
    {
        Task<CompanyTask?> GetByJobIdAsync(int jobId);
        Task<IEnumerable<CompanyTask>> GetTasksByCompanyIdAsync(int companyId);
        Task<IEnumerable<CompanyTask>> GetActiveTasksAsync();
        Task<IEnumerable<CompanyTask>> GetTasksByDeadlineAsync(DateTime deadline);
    }
}
