namespace Fit4Job.DTOs.CompanyTasksDTOs
{
    public class CreateTaskSubmission
    {
        [Display(Name = "Task ID")]
        [Required(ErrorMessage = "Task ID is required")]
        public int TaskId { get; set; }

        [Display(Name = "User ID")]
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Display(Name = "Job Application ID")]
        [Required(ErrorMessage = "Job Application ID is required")]
        public int JobApplicationId { get; set; }

        [Display(Name = "Submission Notes")]
        [StringLength(4000, ErrorMessage = "Submission notes cannot exceed 4,000 characters")]
        public string? SubmissionNotes { get; set; }

        [Display(Name = "Submission Link")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        [StringLength(500, ErrorMessage = "Submission link cannot exceed 500 characters")]
        public string SubmissionLink { get; set; } = string.Empty;

        [Display(Name = "Demo Link")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        [StringLength(500, ErrorMessage = "Demo link cannot exceed 500 characters")]
        public string? DemoLink { get; set; }


        public CompanyTaskSubmission ToEntity()
        {
            return new CompanyTaskSubmission
            {
                TaskId = this.TaskId,
                UserId = this.UserId,
                JobApplicationId = this.JobApplicationId,
                SubmissionNotes = this.SubmissionNotes,
                SubmissionLink = this.SubmissionLink,
                DemoLink = this.DemoLink,
                SubmittedAt = DateTime.UtcNow
            };
        }
    }
}
