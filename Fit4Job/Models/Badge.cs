namespace Fit4Job.Models
{
    [Table("badges")]
    [Index(nameof(TrackId), Name = "IX_Badges_TrackId")]
    public class Badge
    {

        [Key]
        [Display(Name = "Badge ID")]
        public int Id { get; set; }


        [Required(ErrorMessage = "Track ID is required")]
        [Display(Name = "Track ID")]
        public int TrackId { get; set; }


        [Required(ErrorMessage = "Badge name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Badge name must be between 2 and 50 characters")]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Badge Name")]
        public string Name { get; set; }


        [StringLength(1000)]
        [Display(Name = "Description")]
        [Column(TypeName = "text")]
        public string? Description { get; set; }


        [Url(ErrorMessage = "Please enter a valid URL")]
        [StringLength(500, ErrorMessage = "Icon URL cannot exceed 500 characters")]
        [Column(TypeName = "varchar(500)")]
        [Display(Name = "Icon URL")]
        public string? IconUrl { get; set; }


        [Range(0, 99999999.99, ErrorMessage = "Points must be a positive value")]
        [Display(Name = "Points Required")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PointsRequired { get; set; } = 0;


        [Required(ErrorMessage = "Created date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [Required(ErrorMessage = "Updated date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }


        // Computed property
        [NotMapped]
        public bool IsActive => DeletedAt == null;


        // Navigation property
        [ForeignKey("TrackId")]
        public virtual Track Track { get; set; } = null!;

        public virtual ICollection<UserBadge>? UserBadges { get; set; }
    }
}
