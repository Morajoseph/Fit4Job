using Fit4Job.DTOs.CompanyExamsDto;

namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyExamRepository : IGenericRepository<CompanyExam>
    {
        Task<CompanyExam?> GetExamWithDetailsAsync(int id);
        Task<IEnumerable<CompanyExam>> SearchCompanyExamsAsync(CompanyExamSearchDTO companyExamSearchDTO);

        Task<IEnumerable<CompanyExam>> GetActiveExamsByCompanyIdAsync(int companyId);

        Task<IEnumerable<CompanyExam>> GetAvailableExamsByCompanyIdAsync(int companyId);

        Task<CompanyExam?> GetExamWithQuestionsAsync(int examId);

        Task<IEnumerable<CompanyExam>> GetRunningExamsAsync();

        Task<bool> SoftDeleteAsync(int examId);

        Task<bool> RestoreAsync(int examId);
    }
}