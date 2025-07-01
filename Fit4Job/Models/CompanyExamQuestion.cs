namespace Fit4Job.Models
{
    [Table("company_exam_questions")]
    [Index(nameof(ExamId), Name = "IX_CompanyExamQuestions_ExamId")]
    [Index(nameof(QuestionType), Name = "IX_CompanyExamQuestions_QuestionType")]
    [Index(nameof(IsActive), Name = "IX_CompanyExamQuestions_IsActive")]
    public class CompanyExamQuestion
    {
        [Key]
        [Display(Name = "Question ID")]
        public int Id { get; set; }


        [Required(ErrorMessage = "Exam ID is required")]
        [Display(Name = "Exam ID", Description = "Reference to the parent exam")]
        public int ExamId { get; set; }


        [Required(ErrorMessage = "Question text is required")]
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Question Text", Description = "The main question content")]
        [StringLength(10000, MinimumLength = 10, ErrorMessage = "Question text must be between 10 and 10,000 characters")]
        [DataType(DataType.MultilineText)]
        public string QuestionText { get; set; } = string.Empty;


        [Required(ErrorMessage = "Question type is required")]
        [Display(Name = "Question Type", Description = "Type of question (Multiple Choice, Written, etc.)")]
        [Column(TypeName = "varchar(30)")]
        [EnumDataType(typeof(QuestionType), ErrorMessage = "Invalid question type")]
        public QuestionType QuestionType { get; set; }


        [Required(ErrorMessage = "Points value is required")]
        [Range(0.01, 999.99, ErrorMessage = "Points must be between 0.01 and 999.99")]
        [Display(Name = "Points", Description = "Points awarded for correct answer")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Points { get; set; }


        [Column(TypeName = "nvarchar(5000)")]
        [Display(Name = "Explanation", Description = "Optional explanation for the correct answer")]
        [StringLength(5000, ErrorMessage = "Explanation cannot exceed 5,000 characters")]
        [DataType(DataType.MultilineText)]
        public string? Explanation { get; set; }


        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Code Snippet", Description = "Code example for programming questions")]
        [StringLength(10000, ErrorMessage = "Code snippet cannot exceed 10,000 characters")]
        [DataType(DataType.MultilineText)]
        public string? CodeSnippet { get; set; }


        [Column(TypeName = "nvarchar(5000)")]
        [Display(Name = "Expected Output", Description = "Expected output for code execution questions")]
        [StringLength(5000, ErrorMessage = "Expected output cannot exceed 5,000 characters")]
        [DataType(DataType.MultilineText)]
        public string? ExpectedOutput { get; set; }


        [Required]
        [Display(Name = "Is Required", Description = "Whether this question must be answered")]
        public bool IsRequired { get; set; } = false;


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }

        // Computed properties
        [NotMapped]
        public bool IsActive => DeletedAt == null;


        [NotMapped]
        [Display(Name = "Is Multiple Choice")]
        public bool IsMultipleChoice => QuestionType == QuestionType.MultipleChoiceSingle || QuestionType == QuestionType.MultipleChoiceMultiple || QuestionType == QuestionType.TrueFalse;


        [NotMapped]
        [Display(Name = "Is Text Based")]
        public bool IsTextBased => QuestionType == QuestionType.Written || QuestionType == QuestionType.FillInTheBlank || QuestionType == QuestionType.CodeOutput;


        // Navigation properties
        [ForeignKey("ExamId")]
        public virtual CompanyExam Exam { get; set; } = null!;

        public virtual ICollection<CompanyExamQuestionOption>? Options { get; set; }

        public virtual ICollection<CompanyExamQuestionAnswer>? Answers { get; set; }
    }
}
