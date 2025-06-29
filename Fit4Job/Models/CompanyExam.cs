using System.ComponentModel.Design;

namespace Fit4Job.Models
{
    [Table("company_exams")]
    [Index(nameof(CompanyId), Name = "IX_CompanyExams_CompanyId")]
    [Index(nameof(IsActive), Name = "IX_CompanyExams_IsActive")]
    [Index(nameof(StartDate), nameof(EndDate), Name = "IX_CompanyExams_StartDate_EndDate")]
    public class CompanyExam
    {
        [Key]
        [Display(Name = "Exam ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }


        [Required]
        [StringLength(255)]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;


        [Column(TypeName = "text")]
        [Display(Name = "Description")]
        public string? Description { get; set; }


        [Column(TypeName = "text")]
        [Display(Name = "Instructions")]
        public string? Instructions { get; set; }


        [Required]
        [Display(Name = "Duration (Minutes)")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 minute")]
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


        [Required]
        [Display(Name = "Max Attempts")]
        [Range(1, int.MaxValue, ErrorMessage = "Max attempts must be at least 1")]
        public int MaxAttempts { get; set; } = 1;


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
        [Display(Name = "Company")]
        public virtual CompanyProfile Company { get; set; } = null!;

        public virtual ICollection<CompanyExamAttempt>? Attempts { get; set; }
        public virtual ICollection<CompanyExamQuestion>? Questions { get; set; }
    }
}
