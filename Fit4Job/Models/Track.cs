namespace Fit4Job.Models
{
    [Table("tracks")]
    [Index(nameof(IsPremium), Name = "IX_Tracks_IsPremium")]
    [Index(nameof(CategoryId), Name = "IX_Tracks_CategoryId")]
    public class Track
    {
        [Key]
        [Display(Name = "Track ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Track Category is required")]
        [Display(Name = "Track Category ID")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Creator is required")]
        [Display(Name = "Created By")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid creator")]
        public int CreatorId { get; set; }

        [Required(ErrorMessage = "Track name is required")]
        [StringLength(256)]
        [Display(Name = "Track Name")]
        [Column(TypeName = "nvarchar(256)")]
        public string Name { get; set; }

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        [Column(TypeName = "nvarchar(2000)")]
        [Display(Name = "Track Description")]
        [DataType(DataType.MultilineText)]
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

        [Display(Name = "Track Category")]
        [ForeignKey("CategoryId")]
        public virtual TrackCategory Category { get; set; } = null!;
        public virtual ICollection<Badge>? Badges { get; set; }
        public virtual ICollection<TrackAttempt>? TrackAttempts { get; set; }
        public virtual ICollection<TrackQuestion>? TrackQuestions { get; set; }
    }
}
