namespace Fit4Job.Models
{
    [Table("job_seeker_profiles")]
    [Index(nameof(UserId), IsUnique = true, Name = "IX_JobSeekerProfiles_UserId")]
    public class JobSeekerProfile
    {
        [Key]
        [Display(Name = "Job Seeker Profile ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters")]
        [Display(Name = "First Name")]
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters")]
        [Display(Name = "Last Name")]
        [Column(TypeName = "varchar(100)")]
        public string LastName { get; set; } = string.Empty;


        [StringLength(500, ErrorMessage = "CV file URL cannot exceed 500 characters")]
        [Display(Name = "CV File URL")]
        [Column(TypeName = "nvarchar(500)")]
        public string? CvFileUrl { get; set; }


        [Url(ErrorMessage = "Please enter a valid LinkedIn URL")]
        [StringLength(255, ErrorMessage = "LinkedIn URL cannot exceed 255 characters")]
        [Display(Name = "LinkedIn URL")]
        [Column(TypeName = "nvarchar(255)")]
        public string? LinkedinUrl { get; set; }

        [Url(ErrorMessage = "Please enter a valid GitHub URL")]
        [StringLength(255, ErrorMessage = "GitHub URL cannot exceed 255 characters")]
        [Display(Name = "GitHub URL")]
        [Column(TypeName = "nvarchar(255)")]
        public string? GithubUrl { get; set; }

        [Url(ErrorMessage = "Please enter a valid Portfolio URL")]
        [StringLength(255, ErrorMessage = "Portfolio URL cannot exceed 255 characters")]
        [Display(Name = "Portfolio URL")]
        [Column(TypeName = "nvarchar(255)")]
        public string? PortfolioUrl { get; set; }

        [Range(0, 50)]
        [Display(Name = "Years of Experience")]
        public int ExperienceYears { get; set; } = 0;

        [StringLength(255, ErrorMessage = "Current position cannot exceed 255 characters")]
        [Display(Name = "Current Position")]
        [Column(TypeName = "nvarchar(255)")]
        public string? CurrentPosition { get; set; }


        [Range(0, 999999999.99, ErrorMessage = "Expected salary must be between 0 and 999,999,999.99")]
        [Display(Name = "Expected Salary In USD")]
        [Column(TypeName = "decimal(12,2)")]
        public decimal? ExpectedSalary { get; set; } // at USD 


        [Range(0, int.MaxValue, ErrorMessage = "User credit must be a positive number")]
        [Display(Name = "User Credit")]
        public int UserCredit { get; set; } = 5;
        
        
        [StringLength(255, ErrorMessage = "Location cannot exceed 255 characters")]
        [Display(Name = "Location")]
        [Column(TypeName = "nvarchar(255)")]
        public string? Location { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }

        // Computed property

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";


        [NotMapped]
        public bool IsActive => DeletedAt == null;


        // Navigation property
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}