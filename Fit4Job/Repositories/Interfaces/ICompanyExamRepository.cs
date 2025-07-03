namespace Fit4Job.Repositories.Interfaces
{
    public interface ICompanyExamRepository : IGenericRepository<CompanyExam>
    {

        // Get active exams for a specific company
        Task<IEnumerable<CompanyExam>> GetActiveExamsByCompanyIdAsync(int companyId);

        // Get available exams (active, not deleted, within date range)
        Task<IEnumerable<CompanyExam>> GetAvailableExamsByCompanyIdAsync(int companyId);

        // Get exam with questions included
        Task<CompanyExam?> GetExamWithQuestionsAsync(int examId);

        // Get exam with full details (questions and attempts)
        Task<CompanyExam?> GetExamWithFullDetailsAsync(int examId);

        // Get exams by date range
        Task<IEnumerable<CompanyExam>> GetExamsByDateRangeAsync(DateTime startDate, DateTime endDate);

        // Get exams that are currently running
        Task<IEnumerable<CompanyExam>> GetRunningExamsAsync();

        // Get exams that have ended
        Task<IEnumerable<CompanyExam>> GetEndedExamsAsync();

        // Get scheduled exams (future exams)
        Task<IEnumerable<CompanyExam>> GetScheduledExamsAsync();

        // Search exams by title
        Task<IEnumerable<CompanyExam>> SearchExamsByTitleAsync(string searchTerm);

        // Soft delete exam
        Task<bool> SoftDeleteAsync(int examId);

        // Restore soft deleted exam
        Task<bool> RestoreAsync(int examId);

        // Check if exam title exists for company
        Task<bool> ExamTitleExistsAsync(int companyId, string title, int? excludeExamId = null);
    }
}
