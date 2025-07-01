namespace Fit4Job.Models
{
    [Table("notifications")]
    [Index(nameof(UserId), Name = "IX_Notifications_UserId")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public int UserId { get; set; }


        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters")]
        [Display(Name = "Title")]
        [Column(TypeName = "nvarchar(255)")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Message must be between 1 and 1000 characters")]
        [Display(Name = "Message")]
        [Column(TypeName = "nvarchar(1000)")]
        public string Message { get; set; }


        [Url(ErrorMessage = "Please enter a valid URL")]
        [StringLength(500, ErrorMessage = "Action URL cannot exceed 500 characters")]
        [Display(Name = "Action URL")]
        [Column(TypeName = "nvarchar(500)")]
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
        public virtual ApplicationUser User { get; set; } = null!;
    }
}