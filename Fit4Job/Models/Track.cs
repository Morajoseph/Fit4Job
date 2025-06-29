namespace Fit4Job.Models
{
    [Table("tracks")]
    [Index(nameof(IsActive), Name = "IX_Tracks_IsActive")]
    [Index(nameof(IsPremium), Name = "IX_Tracks_IsPremium")]
    [Index(nameof(CategoryId), Name = "IX_Tracks_CategoryId")]
    public class Track
    {

        [Key]
        [Display(Name = "Track ID")]
        public int Id { get; set; }


        [Required]
        [Display(Name = "Track Category ID")]
        public int CategoryId { get; set; }


        [Required]
        [Display(Name = "Created By")]
        public int CreatorId { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "Track Name")]
        public string Name { get; set; }


        [Column(TypeName = "text")]
        [Display(Name = "Track Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Is Premium")]
        public bool IsPremium { get; set; } = false;

        [Required]
        [Range(0, 99999999.99)]
        [Display(Name = "Price")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Price { get; set; } = 0;

        [Required]
        [Display(Name = "Track Questions")]
        public int TrackQuestionsCount { get; set; } = 0;

        [Required]
        [Range(0, 99999999.99)]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Track Total Score")]
        public decimal TrackTotalScore { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
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
        [ForeignKey("CreatorId")]
        public virtual ApplicationUser Creator { get; set; } = null!;


        [ForeignKey("CategoryId")]
        [Display(Name = "Track Category")]
        public virtual TrackCategory Category { get; set; } = null!;


        public virtual ICollection<TrackAttempt>? TrackAttempts { get; set; }
        public virtual ICollection<TrackQuestion>? TrackQuestions { get; set; }
    }
}
