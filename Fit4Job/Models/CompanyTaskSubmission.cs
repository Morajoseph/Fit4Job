namespace Fit4Job.Models
{
    [Table("company_task_submissions")]
    [Index(nameof(TaskId), Name = "IX_CompanyTaskSubmissions_TaskId")]
    [Index(nameof(UserId), Name = "IX_CompanyTaskSubmissions_UserId")]
    [Index(nameof(TaskId), nameof(UserId), IsUnique = true, Name = "IX_CompanyTaskSubmissions_TaskId_UserId")]
    public class CompanyTaskSubmission
    {
        [Key]
        [Display(Name = "Submission ID")]
        public int Id { get; set; }


        [Required(ErrorMessage = "Task ID is required")]
        [Display(Name = "Task ID")]
        public int TaskId { get; set; }


        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public int UserId { get; set; }


        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Submission Notes")]
        [StringLength(4000, ErrorMessage = "Submission notes cannot exceed 4,000 characters")]
        public string? SubmissionNotes { get; set; }


        [Url(ErrorMessage = "Please enter a valid URL")]
        [Required(ErrorMessage = "Submission link is required")]
        [StringLength(500, ErrorMessage = "Submission link cannot exceed 500 characters")]
        [Display(Name = "Submission Link")]
        [Column(TypeName = "nvarchar(500)")]
        public string SubmissionLink { get; set; } = string.Empty;


        [Url(ErrorMessage = "Please enter a valid URL")]
        [StringLength(500, ErrorMessage = "Demo link cannot exceed 500 characters")]
        [Display(Name = "Demo Link")]
        [Column(TypeName = "nvarchar(500)")]
        public string? DemoLink { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Submitted At")]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;


        // Computed properties

        [NotMapped]
        [Display(Name = "Is GitHub Link")]
        public bool IsGitHubLink => SubmissionLink.Contains("github.com", StringComparison.OrdinalIgnoreCase);

        [NotMapped]
        [Display(Name = "Is Google Drive Link")]
        public bool IsGoogleDriveLink => SubmissionLink.Contains("drive.google.com", StringComparison.OrdinalIgnoreCase) ||
                                        SubmissionLink.Contains("docs.google.com", StringComparison.OrdinalIgnoreCase);

        [NotMapped]
        [Display(Name = "Submission Type")]
        public string SubmissionType
        {
            get
            {
                if (IsGitHubLink) return "GitHub Repository";
                if (IsGoogleDriveLink) return "Google Drive";
                return "External Link";
            }
        }


        // Navigation properties
        [ForeignKey("TaskId")]
        public virtual CompanyTask Task { get; set; } = null!;


        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
