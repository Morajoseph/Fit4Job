namespace Fit4Job.Models
{
    [Table("company_profiles")]
    [Index(nameof(UserId), IsUnique = true, Name = "IX_CompanyProfiles_UserId")]
    [Index(nameof(CompanySize), Name = "IX_CompanyProfiles_CompanySize")]
    public class CompanyProfile
    {
        [Key]
        [Display(Name = "Company ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID", Description = "Reference to the associated user account")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive number")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 255 characters")]
        [Display(Name = "Company Name", Description = "Official name of the company")]
        public string CompanyName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(5000)")]
        [Display(Name = "Company Description", Description = "Detailed description of the company and its services")]
        [StringLength(5000, ErrorMessage = "Company description cannot exceed 5,000 characters")]
        [DataType(DataType.MultilineText)]
        public string? CompanyDescription { get; set; }


        [Url(ErrorMessage = "Please enter a valid LinkedIn URL")]
        [StringLength(255, ErrorMessage = "LinkedIn URL cannot exceed 255 characters")]
        [Display(Name = "LinkedIn URL", Description = "Company's LinkedIn profile URL")]
        public string? LinkedinUrl { get; set; }


        [Url(ErrorMessage = "Please enter a valid website URL")]
        [StringLength(255, ErrorMessage = "Website URL cannot exceed 255 characters")]
        [Display(Name = "Website URL", Description = "Company's official website URL")]
        public string? WebsiteUrl { get; set; }


        [StringLength(100, MinimumLength = 2, ErrorMessage = "Industry must be between 2 and 100 characters")]
        [Display(Name = "Industry", Description = "Primary industry or sector of the company")]
        public string? Industry { get; set; }


        [Display(Name = "Company Size", Description = "Number of employees in the company")]
        [EnumDataType(typeof(CompanySize), ErrorMessage = "Invalid company size")]
        public CompanySize? CompanySize { get; set; }


        [Range(1800, 3000, ErrorMessage = "Founding year must be between 1800 and 3000")]
        [Display(Name = "Founding Year", Description = "Year when the company was founded")]
        public int? FoundingYear { get; set; }

        [Required]
        [Display(Name = "Status", Description = "Current approval status of the company profile")]
        [EnumDataType(typeof(CompanyStatus), ErrorMessage = "Invalid company status")]
        public CompanyStatus Status { get; set; } = CompanyStatus.Pending;


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }


        // Helper properties
        [NotMapped]
        public bool IsActive => DeletedAt == null;

        [NotMapped]
        public bool IsApproved => Status == CompanyStatus.Approved;

        [NotMapped]
        public bool IsPending => Status == CompanyStatus.Pending;

        [NotMapped]
        [Display(Name = "Company Age")]
        public int? CompanyAge => FoundingYear.HasValue ? DateTime.UtcNow.Year - FoundingYear.Value : null;

        // Navigation property
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<CompanyTask>? Tasks { get; set; }
        public virtual ICollection<CompanyExam>? CompanyExams { get; set; }
    }
}