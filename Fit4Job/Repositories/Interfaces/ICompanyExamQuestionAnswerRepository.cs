namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyExamQuestionAnswerRepository : IGenericRepository<CompanyExamQuestionAnswer>
    {
        // Get all answers for a specific attempt
        Task<IEnumerable<CompanyExamQuestionAnswer>> GetAnswersByAttemptIdAsync(int attemptId);

        // Get all answers for a specific question
        Task<IEnumerable<CompanyExamQuestionAnswer>> GetAnswersByQuestionIdAsync(int questionId);

        // Get a specific answer by attempt and question
        Task<CompanyExamQuestionAnswer?> GetAnswerByAttemptAndQuestionAsync(int attemptId, int questionId);

        // Get correct answers for a specific attempt
        Task<IEnumerable<CompanyExamQuestionAnswer>> GetCorrectAnswersByAttemptIdAsync(int attemptId);

        // Get incorrect answers for a specific attempt
        Task<IEnumerable<CompanyExamQuestionAnswer>> GetIncorrectAnswersByAttemptIdAsync(int attemptId);

        // Get answers with navigation properties loaded
        Task<IEnumerable<CompanyExamQuestionAnswer>> GetAnswersWithDetailsAsync(int attemptId);

        // Check if an answer exists for a specific attempt and question
        Task<bool> AnswerExistsAsync(int attemptId, int questionId);

        // Update answer score
        Task UpdateAnswerScoreAsync(int answerId, decimal pointsEarned, bool isCorrect);

        // Get unanswered questions for an attempt
        Task<IEnumerable<int>> GetUnansweredQuestionIdsAsync(int attemptId, IEnumerable<int> allQuestionIds);

        // Get answers with text responses only
        Task<IEnumerable<CompanyExamQuestionAnswer>> GetTextAnswersAsync(int attemptId);

        // Get answers with multiple choice responses only
        Task<IEnumerable<CompanyExamQuestionAnswer>> GetMultipleChoiceAnswersAsync(int attemptId);
    }
}