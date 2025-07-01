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


        [Required(ErrorMessage = "Company ID is required")]
        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        [StringLength(256, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 256 characters")]
        [Column(TypeName = "varchar(256)")]
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
        [Column(TypeName = "int")]
        public int DurationMinutes { get; set; }


        [Required(ErrorMessage = "Total score is required")]
        [Range(0, 99999999.99, ErrorMessage = "Total score must be a positive value")]
        [Display(Name = "Total Score")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalScore { get; set; }


        [Required(ErrorMessage = "Passing score is required")]
        [Range(0, 99999999.99, ErrorMessage = "Passing score must be a positive value")]
        [Display(Name = "Passing Score")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PassingScore { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }


        [Required(ErrorMessage = "Active status is required")]
        [Display(Name = "Is Active")]
        [Column(TypeName = "bit")]
        public bool IsActive { get; set; } = true;


        [Required(ErrorMessage = "Show results immediately setting is required")]
        [Display(Name = "Show Results Immediately")]
        [Column(TypeName = "bit")]
        public bool ShowResultsImmediately { get; set; } = false;


        [Required(ErrorMessage = "Created date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Updated date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        [DataType(DataType.DateTime)]
        [Column(TypeName = "timestamp")]
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
