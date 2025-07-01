namespace Fit4Job.Models
{
    [Table("user_badges")]
    [Index(nameof(UserId), Name = "IX_UserBadges_UserId")]
    [Index(nameof(BadgeId), Name = "IX_UserBadges_BadgeId")]
    [Index(nameof(UserId), nameof(BadgeId), IsUnique = true, Name = "IX_UserBadges_UserId_BadgeId_Unique")]
    public class UserBadge
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public int UserId { get; set; }


        [Required(ErrorMessage = "Badge ID is required")]
        [Display(Name = "Badge ID")]
        public int BadgeId { get; set; }


        [Required(ErrorMessage = "Track Attempt ID is required")]
        [Display(Name = "Track Attempt ID")]
        public int TrackAttemptId { get; set; }


        [Display(Name = "Notes")]
        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        [DataType(DataType.MultilineText)]
        [Column(TypeName = "nvarchar(1000)")]
        public string? Notes { get; set; }


        [Required(ErrorMessage = "Earned At timestamp is required")]
        [Display(Name = "Earned At")]
        [DataType(DataType.DateTime)]
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        [Display(Name = "User")]
        public virtual ApplicationUser User { get; set; } = null!;


        [ForeignKey("BadgeId")]
        [Display(Name = "Badge")]
        public virtual Badge Badge { get; set; } = null!;


        [ForeignKey("TrackAttemptId")]
        [Display(Name = "Track Attempt")]
        public virtual TrackAttempt TrackAttempt { get; set; } = null!;
    }
}