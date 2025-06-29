namespace Fit4Job.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [StringLength(500)]
        [Display(Name = "Profile Picture URL")]
        public string? ProfilePictureURL { get; set; }


        [Column(TypeName = "text")]
        public string? Bio { get; set; }


        [Required]
        [EnumDataType(typeof(UserRole))]
        [Column(TypeName = "varchar(20)")]
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


        public bool IsVerified { get; set; } = false; // For Future Update to add verfication for users


        // Computed properties
        [NotMapped]
        public bool IsActive => DeletedAt == null;

        // Navigation properties
        public virtual AdminProfile? AdminProfile { get; set; }
        public virtual CompanyProfile? CompanyProfile { get; set; }
        public virtual JobSeekerProfile? JobSeekerProfile { get; set; }


        public virtual ICollection<UserSkill>? UserSkills { get; set; }
        public virtual ICollection<TrackAttempt>? TrackAttempts { get; set; }
        public virtual ICollection<CompanyExamAttempt>? CompanyExamAttempts { get; set; }
    }
}
