namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyExamQuestionAnswerRepository : IGenericRepository<CompanyExamQuestionAnswer>
    {
        Task<IEnumerable<CompanyExamQuestionAnswer>> GetAnswersByAttemptIdAsync(int attemptId);

        Task<CompanyExamQuestionAnswer?> GetAnswerByAttemptAndQuestionAsync(int attemptId, int questionId);

        Task<bool> AnswerExistsAsync(int attemptId, int questionId);

        Task UpdateAnswerScoreAsync(int answerId, decimal pointsEarned, bool isCorrect);

        Task<IEnumerable<CompanyExamQuestionAnswer>> GetTextAnswersAsync(int attemptId);

        Task<IEnumerable<CompanyExamQuestionAnswer>> GetMultipleChoiceAnswersAsync(int attemptId);
    }
}