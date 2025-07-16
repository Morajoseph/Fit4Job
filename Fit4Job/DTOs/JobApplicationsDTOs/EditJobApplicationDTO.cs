namespace Fit4Job.DTOs.JobApplicationsDTOs
{
    public class EditJobApplicationDTO
    {
        [Display(Name = "Exam Attempt Id")]
        public int? ExamAttemptId { get; set; }

        [Display(Name = "Task Submission Id")]
        public int? TaskSubmissionId { get; set; }

        public void UpdateEntity(JobApplication jobApplication)
        {
            if(jobApplication.ExamAttemptId  == null || jobApplication.ExamAttemptId == 0)
            {
                jobApplication.ExamAttemptId = ExamAttemptId;
            }
            if(jobApplication.TaskSubmissionId == null || jobApplication.TaskSubmissionId == 0)
            {
                jobApplication.TaskSubmissionId = TaskSubmissionId;
            }
            jobApplication.UpdatedAt = DateTime.UtcNow;
        }
    }
}
