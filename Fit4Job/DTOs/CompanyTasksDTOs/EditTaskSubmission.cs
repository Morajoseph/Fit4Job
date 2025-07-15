namespace Fit4Job.DTOs.CompanyTasksDTOs
{
    public class EditTaskSubmission
    {
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

        public void UpdateEntity(CompanyTaskSubmission submission)
        {
            submission.SubmissionNotes = SubmissionNotes;
            submission.SubmissionLink = SubmissionLink;
            submission.DemoLink = DemoLink;
            submission.SubmittedAt = DateTime.UtcNow; 
        }
    }
}
