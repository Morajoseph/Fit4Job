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


        [Required]
        [Display(Name = "Task ID")]
        public int TaskId { get; set; }


        [Required]
        [Display(Name = "User ID")]
        public int UserId { get; set; }


        [Column(TypeName = "text")]
        [Display(Name = "Submission Notes")]
        public string? SubmissionNotes { get; set; }


        [Url]
        [Required]
        [StringLength(500)]
        [Display(Name = "Submission Link")]
        public string SubmissionLink { get; set; } = string.Empty;


        [Url]
        [StringLength(500)]
        [Display(Name = "Demo Link")]
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
        [Display(Name = "Task")]
        [ForeignKey("TaskId")]
        public virtual CompanyTask Task { get; set; } = null!;


        [Display(Name = "User")]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        // Helper methods

        //public bool IsValidSubmissionLink()
        //{
        //    return Uri.IsWellFormedUriString(SubmissionLink, UriKind.Absolute);
        //}

        //public bool IsValidDemoLink()
        //{
        //    return string.IsNullOrWhiteSpace(DemoLink) ||
        //           Uri.IsWellFormedUriString(DemoLink, UriKind.Absolute);
        //}
    }
}
