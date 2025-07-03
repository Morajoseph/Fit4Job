namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyExamQuestionRepository : IGenericRepository<CompanyExamQuestion>
    {
        // Get questions by exam ID
        Task<IEnumerable<CompanyExamQuestion>> GetByExamIdAsync(int examId);

        // Get active questions by exam ID
        Task<IEnumerable<CompanyExamQuestion>> GetActiveQuestionsByExamIdAsync(int examId);

        // Get questions by exam ID and type
        Task<IEnumerable<CompanyExamQuestion>> GetByExamIdAndTypeAsync(int examId, QuestionType questionType);

        // Get questions with their options (for multiple choice questions)
        Task<IEnumerable<CompanyExamQuestion>> GetQuestionsWithOptionsAsync(int examId);

        // Get questions with their answers
        Task<IEnumerable<CompanyExamQuestion>> GetQuestionsWithAnswersAsync(int examId);

        // Get complete question data (with options and answers)
        Task<CompanyExamQuestion?> GetCompleteQuestionAsync(int questionId);

        // Get total points for an exam
        Task<decimal> GetTotalPointsByExamIdAsync(int examId);

        // Count questions by exam
        Task<int> CountQuestionsByExamIdAsync(int examId);

        // Soft delete question
        Task<bool> SoftDeleteAsync(int questionId);

        // Restore soft deleted question
        Task<bool> RestoreAsync(int questionId);
    }
}
