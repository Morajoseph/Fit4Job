namespace Fit4Job.Models
{
    [Table("company_exam_attempts")]
    [Index(nameof(UserId), Name = "IX_CompanyExamAttempts_UserId")]
    [Index(nameof(ExamId), Name = "IX_CompanyExamAttempts_ExamId")]
    [Index(nameof(Status), Name = "IX_CompanyExamAttempts_Status")]
    public class CompanyExamAttempt
    {
        [Key]
        [Display(Name = "Attempt ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Exam ID is required")]
        [Display(Name = "Exam ID")]
        public int ExamId { get; set; }


        [Required(ErrorMessage = "Start time is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;


        [DataType(DataType.DateTime)]
        [Display(Name = "End Time")]
        public DateTime? EndTime { get; set; }


        [Required(ErrorMessage = "Score is required")]
        [Range(0, 99999999.99, ErrorMessage = "Score must be a positive value")]
        [Display(Name = "Total Score")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Score { get; set; } = 0.00m;


        [Range(0, 100.00, ErrorMessage = "Percentage score must be between 0 and 100")]
        [Display(Name = "Percentage Score")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PercentageScore { get; set; } = 0.00m;


        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        [Column(TypeName = "varchar(15)")]
        [EnumDataType(typeof(CompanyExamAttemptStatus), ErrorMessage = "Please select a valid status")]
        public CompanyExamAttemptStatus Status { get; set; } = CompanyExamAttemptStatus.InProgress;

        
        [Required(ErrorMessage = "Passed status is required")]
        [Display(Name = "Passed")]
        [Column(TypeName = "bit")]
        public bool Passed { get; set; } = false;

        
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


        // Computed properties
        [NotMapped]
        [Display(Name = "Duration")]
        public TimeSpan? Duration => EndTime.HasValue ? EndTime.Value - StartTime : null;


        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey("ExamId")]
        public virtual CompanyExam Exam { get; set; } = null!;

        public virtual ICollection<CompanyExamQuestionAnswer>? Answers { get; set; }
    }
}