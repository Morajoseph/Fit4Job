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


        [Required]
        [Display(Name = "Exam ID")]
        public int ExamId { get; set; }


        [Required]
        [Column(TypeName = "text")]
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; } = string.Empty;


        [Required]
        [Display(Name = "Question Type")]
        [Column(TypeName = "varchar(30)")]
        [EnumDataType(typeof(QuestionType))]
        public QuestionType QuestionType { get; set; }


        [Required]
        [Range(0, 999.99)]
        [Display(Name = "Points")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Points { get; set; }


        [Column(TypeName = "text")]
        [Display(Name = "Explanation")]
        public string? Explanation { get; set; }


        [Column(TypeName = "text")]
        [Display(Name = "Code Snippet")]
        public string? CodeSnippet { get; set; }


        [Column(TypeName = "text")]
        [Display(Name = "Expected Output")]
        public string? ExpectedOutput { get; set; }


        [Required]
        [Display(Name = "Is Required")]
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
