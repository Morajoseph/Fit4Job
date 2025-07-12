namespace Fit4Job.Models
{
    [Index(nameof(UserId), nameof(JobId), IsUnique = true)]
    public class JobApplication
    {
        [Key]
        [Display(Name = "Job Application ID")]
        public int Id { get; set; }

        [Display(Name = "User ID")]
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Display(Name = "Job ID")]
        [Required(ErrorMessage = "Job ID is required")]
        public int JobId { get; set; }

        [Display(Name = "Exam Attempt Id")]
        public int? ExamAttemptId { get; set; }

        [Display(Name = "Task Submission Id")]
        public int? TaskSubmissionId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Applied At")]
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        
        [ForeignKey("ExamAttemptId")]
        public virtual CompanyExamAttempt? ExamAttempt { get; set; }
        
        [ForeignKey("TaskSubmissionId")]
        public virtual CompanyTaskSubmission? TaskSubmission { get; set; }   
    }
}
