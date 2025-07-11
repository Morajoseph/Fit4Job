namespace Fit4Job.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Url(ErrorMessage = "Please enter a valid URL")]
        [StringLength(500, ErrorMessage = "Profile picture URL cannot exceed 500 characters")]
        [Column(TypeName = "nvarchar(500)")]
        [Display(Name = "Profile Picture URL")]
        public string? ProfilePictureURL { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        [StringLength(500, ErrorMessage = "Profile picture URL cannot exceed 500 characters")]
        [Column(TypeName = "nvarchar(500)")]
        [Display(Name = "Cover Picture URL")]
        public string? CoverPictureURL { get; set; }

        [StringLength(2000, ErrorMessage = "Bio cannot exceed 2000 characters")]
        [Column(TypeName = "nvarchar(2000)")]
        [Display(Name = "Bio")]
        public string? Bio { get; set; }


        [Required(ErrorMessage = "User role is required")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Please select a valid user role")]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Role")]
        public UserRole Role { get; set; }


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

        [Display(Name = "Is Verified")]
        public bool IsVerified { get; set; } = false; // For Future Update to add verfication for users

        // Computed properties
        [NotMapped]
        public bool IsActive => DeletedAt == null;

        // Navigation properties
        public virtual AdminProfile? AdminProfile { get; set; }
        public virtual CompanyProfile? CompanyProfile { get; set; }
        public virtual JobSeekerProfile? JobSeekerProfile { get; set; }
        public virtual ICollection<CompanyTaskSubmission>? CompanyTaskSubmissions { get; set; }
        public virtual ICollection<Track>? CreatedTracks { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
        public virtual ICollection<UserSkill>? UserSkills { get; set; }
        public virtual ICollection<TrackAttempt>? TrackAttempts { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<CompanyExamAttempt>? CompanyExamAttempts { get; set; }
        public virtual ICollection<UserBadge>? UserBadges { get; set; }
    }
}