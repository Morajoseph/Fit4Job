namespace Fit4Job.Models
{

    [Table("badges")]
    [Index(nameof(TrackId), Name = "IX_Badges_TrackId")]
    [Index(nameof(IsActive), Name = "IX_Badges_IsActive")]
    public class Badge
    {

        [Key]
        [Display(Name = "Badge ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Badge Name")]
        public string Name { get; set; } 

        [Display(Name = "Description")]
        [Column(TypeName = "text")]
        public string? Description { get; set; }

        [Url]
        [Display(Name = "Icon URL")]
        [StringLength(500)]
        public string? IconUrl { get; set; }

        [Display(Name = "Points Required")]
        public int PointsRequired { get; set; } = 0;

        [Display(Name = "Track ID")]
        public int? TrackId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }



        // Computed property
        [NotMapped]
        public bool IsActive => DeletedAt == null;

        // Navigation

        [ForeignKey("TrackId")]
        public virtual Track? Track { get; set; }

        public virtual ICollection<UserBadge>? UserBadges { get; set; }
    }

}
