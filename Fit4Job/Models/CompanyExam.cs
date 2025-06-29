namespace Fit4Job.Models
{
    [Table("company_exams")]
    [Index(nameof(IsActive), Name = "IX_CompanyExams_IsActive")]
    [Index(nameof(CompanyId), Name = "IX_CompanyExams_CompanyId")]
    [Index(nameof(StartDate), nameof(EndDate), Name = "IX_CompanyExams_StartDate_EndDate")]
    public class CompanyExam
    {
        [Key]
        [Display(Name = "Exam ID")]
        public int Id { get; set; }


        [Required]
        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }


        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        [StringLength(256, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 256 characters")]
        public string Title { get; set; } = null!;


        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Column(TypeName = "varchar(500)")]
        public string? Description { get; set; }


        [Display(Name = "Instructions")]
        [StringLength(2048, ErrorMessage = "Instructions cannot exceed 2048 characters")]
        [Column(TypeName = "varchar(2048)")]
        public string? Instructions { get; set; }


        [Required(ErrorMessage = "Duration is required")]
        [Display(Name = "Duration (Minutes)")]
        [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes (10 hours)")]
        public int DurationMinutes { get; set; }


        [Required]
        [Range(0, 99999999.99)]
        [Display(Name = "Max Score")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalScore { get; set; }


        [Required]
        [Range(0, 99999999.99)]
        [Display(Name = "Passing Score")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PassingScore { get; set; }


        //[Required]
        //[Display(Name = "Max Attempts")]
        //[Range(1, int.MaxValue, ErrorMessage = "Max attempts must be at least 1")]
        //public int MaxAttempts { get; set; } = 1;


        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }


        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;


        [Required]
        [Display(Name = "Show Results Immediately")]
        public bool ShowResultsImmediately { get; set; } = false;


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


        // Computed properties
        [NotMapped]
        [Display(Name = "Duration")]
        public TimeSpan Duration => TimeSpan.FromMinutes(DurationMinutes);

        [NotMapped]
        [Display(Name = "Is Available")]
        public bool IsAvailable
        {
            get
            {
                var now = DateTime.UtcNow;
                return IsActive && DeletedAt == null &&
                       (StartDate == null || StartDate <= now) &&
                       (EndDate == null || EndDate >= now);
            }
        }

        [NotMapped]
        [Display(Name = "Is Scheduled")]
        public bool IsScheduled => StartDate.HasValue || EndDate.HasValue;


        [NotMapped]
        [Display(Name = "Has Ended")]
        public bool HasEnded => EndDate.HasValue && EndDate < DateTime.UtcNow;


        [NotMapped]
        [Display(Name = "Has Started")]
        public bool HasStarted => StartDate == null || StartDate <= DateTime.UtcNow;


        // Navigation properties
        [ForeignKey("CompanyId")]
        public virtual CompanyProfile Company { get; set; } = null!;
        public virtual ICollection<CompanyExamAttempt>? Attempts { get; set; }
        public virtual ICollection<CompanyExamQuestion>? Questions { get; set; }
    }
}
