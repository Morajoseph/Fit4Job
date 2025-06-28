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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BadgeId { get; set; }

        public int? PracticeAttemptId { get; set; }

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Earned At")]
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey("BadgeId")]
        public virtual Badge Badge { get; set; } = null!;

        //[ForeignKey("PracticeAttemptId")]
        //public virtual TrackAttempt? PracticeAttempt { get; set; }
    }
}
