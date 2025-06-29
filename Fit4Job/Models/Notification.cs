namespace Fit4Job.Models
{
    [Table("notifications")]
    [Index(nameof(UserId), Name = "IX_Notifications_UserId")]
    [Index(nameof(IsRead), Name = "IX_Notifications_IsRead")]
    [Index(nameof(CreatedAt), Name = "IX_Notifications_CreatedAt")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int UserId { get; set; }


        [Required]
        [StringLength(255)]
        public string Title { get; set; }


        [Required]
        [Column(TypeName = "text")]
        public string Message { get; set; }


        [Url]
        [Display(Name = "Action URL")]
        [StringLength(500)]
        public string? ActionUrl { get; set; }


        [Display(Name = "Is Read")]
        public bool IsRead { get; set; } = false;


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }


        [Display(Name = "Read At")]
        [DataType(DataType.DateTime)]
        public DateTime? ReadAt { get; set; }


        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}