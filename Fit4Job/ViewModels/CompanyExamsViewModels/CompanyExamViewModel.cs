namespace Fit4Job.ViewModels.CompanyExamsViewModel
{
    public class CompanyExamViewModel
    {

        [Display(Name = "Exam ID")]
        public int Id { get; set; }

        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }

        [Display(Name = "Job ID")]
        public int JobId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Instructions")]
        public string? Instructions { get; set; }

        [Display(Name = "Duration (Minutes)")]
        public int DurationMinutes { get; set; }

        [Display(Name = "Total Score")]
        public decimal TotalScore { get; set; }

        [Display(Name = "Passing Score")]
        public decimal PassingScore { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Show Results Immediately")]
        public bool ShowResultsImmediately { get; set; }

        public CompanyExamViewModel() 
        {
            
        }

        public CompanyExamViewModel(CompanyExam exam)
        {
            Id = exam.Id;
            CompanyId = exam.CompanyId;
            JobId = exam.JobId;
            Title = exam.Title;
            Description = exam.Description;
            Instructions = exam.Instructions;
            DurationMinutes = exam.DurationMinutes;
            TotalScore = exam.TotalScore;
            PassingScore = exam.PassingScore;
            IsActive = exam.IsActive;
            ShowResultsImmediately = exam.ShowResultsImmediately;
        }

        public static CompanyExamViewModel GetViewModel(CompanyExam exam)
        {
            return new CompanyExamViewModel(exam);
        }
    }
}
