namespace Fit4Job.Models
{
    [Table("job_seeker_profiles")]
    [Index(nameof(UserId), IsUnique = true, Name = "IX_JobSeekerProfiles_UserId")]
    public class JobSeekerProfile
    {
        [Key]
        [Display(Name = "Job Seeker Profile ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "CV File URL")]
        public string? CvFileUrl { get; set; }

        [Url]
        [StringLength(255)]
        [Display(Name = "LinkedIn URL")]
        public string? LinkedinUrl { get; set; }

        [Url]
        [StringLength(255)]
        [Display(Name = "GitHub URL")]
        public string? GithubUrl { get; set; }

        [Url]
        [StringLength(255)]
        [Display(Name = "Portfolio URL")]
        public string? PortfolioUrl { get; set; }

        [Range(0, 50)]
        [Display(Name = "Years of Experience")]
        public int ExperienceYears { get; set; } = 0;

        [StringLength(255)]
        [Display(Name = "Current Position")]
        public string? CurrentPosition { get; set; }

        [Range(0, 999999999.99)]
        [Display(Name = "Expected Salary")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? ExpectedSalary { get; set; }


        [Range(0, int.MaxValue)]
        [Display(Name = "User Credit")]
        public int UserCredit { get; set; } = 5;

        [StringLength(255)]
        [Display(Name = "Location")]
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