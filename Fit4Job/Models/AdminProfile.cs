namespace Fit4Job.Models
{
    [Table("admin_profiles")]
    [Index(nameof(UserId), IsUnique = true, Name = "IX_AdminProfiles_UserId")]
    public class AdminProfile
    {
        [Key]
        [Display(Name = "Admin ID")]
        public int Id { get; set; }


        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public int UserId { get; set; }


        [Required(ErrorMessage = "First name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 20 characters")]
        [Display(Name = "First Name", Description = "Administrator's first name")]
        public string FirstName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Last name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 20 characters")]
        [Display(Name = "Last Name", Description = "Administrator's last name")]
        public string LastName { get; set; } = string.Empty;


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