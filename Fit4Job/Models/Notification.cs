namespace Fit4Job.Models
{

    [Table("notifications")]
    [Index(nameof(UserId), Name = "IX_Notifications_UserId")]
    [Index(nameof(IsRead), Name = "IX_Notifications_IsRead")]
    [Index(nameof(CreatedAt), Name = "IX_Notifications_CreatedAt")]
    [Index(nameof(UserId), nameof(IsRead), nameof(CreatedAt), Name = "IX_Notifications_UserId_IsRead_CreatedAt")]
    public class Notification:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "text")]
        public string Message { get; set; } = string.Empty;

        [Url]
        [Display(Name = "Action URL")]
        [StringLength(500)]
        public string? ActionUrl { get; set; }

        [Display(Name = "Is Read")]
        public bool IsRead { get; set; } = false;

     public DateTime? ReadAt { get; set; }


        // Navigation
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

    }
}
