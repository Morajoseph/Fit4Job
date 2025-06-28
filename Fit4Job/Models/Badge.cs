namespace Fit4Job.Models
{

    [Table("badges")]
    [Index(nameof(TrackId), Name = "IX_Badges_TrackId")]
    [Index(nameof(IsActive), Name = "IX_Badges_IsActive")]
    public class Badge : BaseEntity
    {

        [Key]
        [Display(Name = "Badge ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Badge Name")]
        public string Name { get; set; } = string.Empty;

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



        // Navigation

        //[ForeignKey("TrackId")]
        //public virtual Track? Track { get; set; }

        public virtual ICollection<UserBadge>? UserBadges { get; set; }
    }

}
