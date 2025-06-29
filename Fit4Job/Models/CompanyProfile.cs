namespace Fit4Job.Models
{
    [Table("company_profiles")]
    [Index(nameof(UserId), IsUnique = true, Name = "IX_CompanyProfiles_UserId")]
    [Index(nameof(Status), Name = "IX_CompanyProfiles_Status")]
    [Index(nameof(Industry), Name = "IX_CompanyProfiles_Industry")]
    [Index(nameof(CompanySize), Name = "IX_CompanyProfiles_CompanySize")]
    public class CompanyProfile
    {
        [Key]
        [Display(Name = "Company ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        [Display(Name = "Company Description")]
        public string? CompanyDescription { get; set; }

        [Url]
        [StringLength(255)]
        [Display(Name = "LinkedIn URL")]
        public string? LinkedinUrl { get; set; }


        [Url]
        [StringLength(255)]
        [Display(Name = "Website URL")]
        public string? WebsiteUrl { get; set; }

        [StringLength(100)]
        [Display(Name = "Industry")]
        public string? Industry { get; set; }


        [Display(Name = "Company Size")]
        public CompanySize? CompanySize { get; set; }

        [Range(1800, 3000)]
        [Display(Name = "Founding Year")]
        public int? FoundingYear { get; set; }

        [Required]
        [Display(Name = "Status")]
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