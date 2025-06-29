namespace Fit4Job.Models
{
    [Table("track_attempts")]
    [Index(nameof(UserId), Name = "IX_TrackAttempts_UserId")]
    [Index(nameof(TrackId), Name = "IX_TrackAttempts_TrackId")]
    [Index(nameof(Status), Name = "IX_TrackAttempts_Status")]
    [Index(nameof(UserId), nameof(TrackId), IsUnique = true, Name = "IX_TrackAttempts_UserId_TrackId")]
    [Index(nameof(UserId), nameof(Status), nameof(StartTime), Name = "IX_TrackAttempts_UserId_Status_StartTime")]
    public class TrackAttempt
    {
        [Key]
        [Display(Name = "Attempt ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Track ID")]
        public int TrackId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        [Display(Name = "End Time")]
        [DataType(DataType.DateTime)]
        public DateTime? EndTime { get; set; }

        [Required]
        [Range(0, 99999999.99)]
        [Display(Name = "Total Score")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalScore { get; set; } = 0.00m;

        [Required]
        [Range(0, 100)]
        [Display(Name = "Progress Percentage")]
        public int ProgressPercentage { get; set; } = 0;

        [Required]
        [Display(Name = "Status")]
        [Column(TypeName = "varchar(15)")]
        [EnumDataType(typeof(AttemptStatus))]
        public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;

        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Solved Questions Count")]
        public int SolvedQuestionsCount { get; set; } = 0;


        // Computed properties
        [NotMapped]
        [Display(Name = "Duration")]
        public TimeSpan? Duration => EndTime.HasValue ? EndTime.Value - StartTime : null;

        [NotMapped]
        [Display(Name = "Is Completed")]
        public bool IsCompleted => Status == AttemptStatus.Completed;

        [NotMapped]
        [Display(Name = "Is In Progress")]
        public bool IsInProgress => Status == AttemptStatus.InProgress;

        // Navigation properties
        [Display(Name = "User")]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;


        [Display(Name = "Track")]
        [ForeignKey("TrackId")]
        public virtual Track Track { get; set; } = null!;


        public virtual ICollection<Badge>? Badges { get; set; }
        
        
        public virtual ICollection<TrackQuestionAnswer>? Answers { get; set; }
    }
}
