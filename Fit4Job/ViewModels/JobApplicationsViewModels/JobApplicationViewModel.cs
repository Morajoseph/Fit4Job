namespace Fit4Job.ViewModels.JobApplicationsViewModels
{
    public class JobApplicationViewModel
    {
        [Display(Name = "Job Application ID")]
        public int Id { get; set; }

        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Display(Name = "Job ID")]
        public int JobId { get; set; }

        [Display(Name = "Exam Attempt Id")]
        public int? ExamAttemptId { get; set; }

        [Display(Name = "Task Submission Id")]
        public int? TaskSubmissionId { get; set; }

        [Display(Name = "Application Status", Description = "Current status of the job application")]
        public JobApplicationStatus Status { get; set; }

        [Display(Name = "Applied At")]
        public DateTime AppliedAt { get; set; }

        public JobApplicationViewModel()
        {

        }

        public JobApplicationViewModel(JobApplication jobApplication)
        {
            Id = jobApplication.Id;
            UserId = jobApplication.UserId;
            JobId = jobApplication.JobId;
            ExamAttemptId = jobApplication.ExamAttemptId;
            TaskSubmissionId = jobApplication.TaskSubmissionId;
            Status = jobApplication.Status;
            AppliedAt = jobApplication.AppliedAt;
        }

        public static JobApplicationViewModel GetViewModel(JobApplication jobApplication)
        {
            return new JobApplicationViewModel(jobApplication);
        }
    }
}
