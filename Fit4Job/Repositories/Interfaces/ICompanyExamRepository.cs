namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyExamRepository : IGenericRepository<CompanyExam>
    {
        Task<IEnumerable<CompanyExam>> GetActiveExamsByCompanyIdAsync(int companyId);

        Task<IEnumerable<CompanyExam>> GetAvailableExamsByCompanyIdAsync(int companyId);

        Task<CompanyExam?> GetExamWithQuestionsAsync(int examId);

        Task<IEnumerable<CompanyExam>> GetRunningExamsAsync();

        Task<bool> SoftDeleteAsync(int examId);

        Task<bool> RestoreAsync(int examId);
    }
}