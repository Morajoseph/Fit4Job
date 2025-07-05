namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyExamQuestionRepository : IGenericRepository<CompanyExamQuestion>
    {
        Task<IEnumerable<CompanyExamQuestion>> GetByExamIdAsync(int examId);

        Task<IEnumerable<CompanyExamQuestion>> GetActiveQuestionsByExamIdAsync(int examId);

        Task<IEnumerable<CompanyExamQuestion>> GetQuestionsWithOptionsAsync(int examId);

        Task<decimal> GetTotalPointsByExamIdAsync(int examId);

        Task<bool> SoftDeleteAsync(int questionId);

        Task<bool> RestoreAsync(int questionId);
    }
}
