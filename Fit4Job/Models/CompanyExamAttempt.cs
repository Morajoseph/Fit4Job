namespace Fit4Job.Models
{
    [Table("company_exam_attempts")]
    [Index(nameof(UserId), Name = "IX_CompanyExamAttempts_UserId")]
    [Index(nameof(ExamId), Name = "IX_CompanyExamAttempts_ExamId")]
    [Index(nameof(Status), Name = "IX_CompanyExamAttempts_Status")]
    [Index(nameof(UserId), nameof(ExamId), nameof(AttemptNumber), IsUnique = true, Name = "IX_CompanyExamAttempts_UserId_ExamId_AttemptNumber")]
    public class CompanyExamAttempt
    {
        [Key]
        [Display(Name = "Attempt ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Exam ID")]
        public int ExamId { get; set; }


        //[Required]
        //[Range(1, int.MaxValue)]
        //[Display(Name = "Attempt Number")]
        //public int AttemptNumber { get; set; } = 1;


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;


        [DataType(DataType.DateTime)]
        [Display(Name = "End Time")]
        public DateTime? EndTime { get; set; }


        [Required]
        [Range(0, 99999999.99)]
        [Display(Name = "Total Score")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Score { get; set; } = 0.00m;


        [Required]
        [Range(0, 100.00)]
        [Display(Name = "Percentage Score")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PercentageScore { get; set; } = 0.00m;


        [Required]
        [Display(Name = "Status")]
        [Column(TypeName = "varchar(15)")]
        [EnumDataType(typeof(CompanyExamAttemptStatus))]
        public CompanyExamAttemptStatus Status { get; set; } = CompanyExamAttemptStatus.InProgress;


        [Required]
        [Display(Name = "Passed")]
        public bool Passed { get; set; } = false;

        // Computed properties
        [NotMapped]
        [Display(Name = "Duration")]
        public TimeSpan? Duration => EndTime.HasValue ? EndTime.Value - StartTime : null;


        // Navigation properties
        [Display(Name = "User")]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        [Display(Name = "Exam")]
        [ForeignKey("ExamId")]
        public virtual CompanyExam Exam { get; set; } = null!;

        public virtual ICollection<CompanyExamQuestionAnswer>? Answers { get; set; }

    }
}