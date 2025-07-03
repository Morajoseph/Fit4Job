namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyExamQuestionOptionRepository : IGenericRepository<CompanyExamQuestionOption>
    {
        // Get all options for a specific question
        Task<IEnumerable<CompanyExamQuestionOption>> GetOptionsByQuestionIdAsync(int questionId);

        // Get all active options for a specific question
        Task<IEnumerable<CompanyExamQuestionOption>> GetActiveOptionsByQuestionIdAsync(int questionId);

        // Get correct options for a specific question
        Task<IEnumerable<CompanyExamQuestionOption>> GetCorrectOptionsByQuestionIdAsync(int questionId);

        // Get incorrect options for a specific question
        Task<IEnumerable<CompanyExamQuestionOption>> GetIncorrectOptionsByQuestionIdAsync(int questionId);

        // Get options with navigation properties loaded
        Task<IEnumerable<CompanyExamQuestionOption>> GetOptionsWithDetailsAsync(int questionId);

        // Get option by ID including question details
        Task<CompanyExamQuestionOption?> GetOptionWithQuestionAsync(int optionId);

        // Soft delete option
        Task<bool> SoftDeleteOptionAsync(int optionId);

        // Restore soft deleted option
        Task<bool> RestoreOptionAsync(int optionId);

        // Get options count for a question
        Task<int> GetOptionsCountAsync(int questionId);

        // Get correct options count for a question
        Task<int> GetCorrectOptionsCountAsync(int questionId);

        // Check if all provided option IDs belong to a specific question
        Task<bool> AllOptionsBelongToQuestionAsync(IEnumerable<int> optionIds, int questionId);

        // Update option correctness
        Task UpdateOptionCorrectnessAsync(int optionId, bool isCorrect);

        // Bulk update options for a question
        Task BulkUpdateOptionsAsync(int questionId, IEnumerable<CompanyExamQuestionOption> options);
    }
}
