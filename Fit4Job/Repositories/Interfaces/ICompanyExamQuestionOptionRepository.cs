namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyExamQuestionOptionRepository : IGenericRepository<CompanyExamQuestionOption>
    {
        Task<IEnumerable<CompanyExamQuestionOption>> GetOptionsByQuestionIdAsync(int questionId);

        Task<IEnumerable<CompanyExamQuestionOption>> GetActiveOptionsByQuestionIdAsync(int questionId);

        Task<IEnumerable<CompanyExamQuestionOption>> GetCorrectOptionsByQuestionIdAsync(int questionId);

        Task<int> GetOptionsCountAsync(int questionId);

        Task<int> GetCorrectOptionsCountAsync(int questionId);

        Task<bool> SoftDeleteOptionAsync(int optionId);

        Task<bool> RestoreOptionAsync(int optionId);
    }
}
