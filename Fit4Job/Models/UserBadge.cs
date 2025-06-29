namespace Fit4Job.Models
{
    [Table("user_badges")]
    [Index(nameof(UserId), Name = "IX_UserBadges_UserId")]
    [Index(nameof(BadgeId), Name = "IX_UserBadges_BadgeId")]
    [Index(nameof(EarnedAt), Name = "IX_UserBadges_EarnedAt")]
    [Index(nameof(UserId), nameof(BadgeId), IsUnique = true, Name = "IX_UserBadges_UserId_BadgeId_Unique")]
    public class UserBadge
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int UserId { get; set; }


        [Required]
        public int BadgeId { get; set; }


        [Required]
        public int TrackAttemptId { get; set; }


        [Display(Name = "Notes")]
        public string? Notes { get; set; }


        [Display(Name = "Earned At")]
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;


        [ForeignKey("BadgeId")]
        public virtual Badge Badge { get; set; } = null!;


        [ForeignKey("TrackAttemptId")]
        public virtual TrackAttempt TrackAttempt { get; set; } = null!;
    }
}