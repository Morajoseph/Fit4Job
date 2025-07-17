namespace Fit4Job.ViewModels.CompanyExamAttemptsViewModels
{
    public class CompanyExamAttemptViewModel
    {
        [Display(Name = "Attempt ID")]
        public int Id { get; set; }

        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Display(Name = "Exam ID")]
        public int ExamId { get; set; }

        [Display(Name = "Job Application ID")]
        public int JobApplicationId { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        [Display(Name = "End Time")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "Total Score")]
        public decimal Score { get; set; }

        [Display(Name = "Percentage Score")]
        public decimal PercentageScore { get; set; } = 0.00m;

        [Display(Name = "Status")]
        public CompanyExamAttemptStatus Status { get; set; } = CompanyExamAttemptStatus.InProgress;

        [Display(Name = "Passed")]
        public bool Passed { get; set; } = false;


        public CompanyExamAttemptViewModel()
        {

        }

        public CompanyExamAttemptViewModel(CompanyExamAttempt examAttempt)
        {
            Id = examAttempt.Id;
            UserId = examAttempt.UserId;
            ExamId = examAttempt.ExamId;
            JobApplicationId = examAttempt.JobApplicationId;
            StartTime = examAttempt.StartTime;
            EndTime = examAttempt.EndTime;
            Score = examAttempt.Score;
            PercentageScore = examAttempt.PercentageScore;
            Status = examAttempt.Status;
            Passed = examAttempt.Passed;
        }
        public static CompanyExamAttemptViewModel GetViewModel(CompanyExamAttempt examAttempt)
        {
            return new CompanyExamAttemptViewModel(examAttempt);
        }
    }
}