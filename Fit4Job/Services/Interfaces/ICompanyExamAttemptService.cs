namespace Fit4Job.Services.Interfaces
{
    public interface ICompanyExamAttemptService
    {
        Task<ApiResponse<CompanyExamAttemptViewModel>> Create(CreateCompanyExamAttemptDTO examAttemptDTO, ApplicationUser user);
        Task<ApiResponse<IEnumerable<CompanyExamAttemptViewModel>>> GetAllByExam(int examId);
        Task<ApiResponse<IEnumerable<CompanyExamAttemptViewModel>>> GetAll();
        Task<ApiResponse<CompanyExamAttemptViewModel>> GetById(int id);
    }
}