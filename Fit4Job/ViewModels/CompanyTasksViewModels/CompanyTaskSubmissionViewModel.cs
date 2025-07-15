namespace Fit4Job.ViewModels.CompanyTasksViewModels
{
    public class CompanyTaskSubmissionViewModel
    {
        [Display(Name = "Submission ID")]
        public int Id { get; set; }

        [Display(Name = "Task ID")]
        public int TaskId { get; set; }

        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Display(Name = "Job Application ID")]
        public int JobApplicationId { get; set; }

        [Display(Name = "Submission Notes")]
        public string? SubmissionNotes { get; set; }

        [Display(Name = "Submission Link")]
        public string SubmissionLink { get; set; } = null!;

        [Display(Name = "Demo Link")]
        public string? DemoLink { get; set; }

        [Display(Name = "Submitted At")]
        public DateTime SubmittedAt { get; set; }

        public CompanyTaskSubmissionViewModel()
        {

        }

        public CompanyTaskSubmissionViewModel(CompanyTaskSubmission taskSubmission)
        {
            Id = taskSubmission.Id;
            TaskId = taskSubmission.TaskId;
            UserId = taskSubmission.UserId;
            JobApplicationId = taskSubmission.JobApplicationId;
            SubmissionNotes = taskSubmission.SubmissionNotes;
            SubmissionLink = taskSubmission.SubmissionLink;
            DemoLink = taskSubmission.DemoLink;
            SubmittedAt = taskSubmission.SubmittedAt;
        }

        public static CompanyTaskSubmissionViewModel GetViewModel(CompanyTaskSubmission taskSubmission)
        {
            return new CompanyTaskSubmissionViewModel(taskSubmission);
        }
    }
}
