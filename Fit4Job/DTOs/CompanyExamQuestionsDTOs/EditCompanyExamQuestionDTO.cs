namespace Fit4Job.DTOs.CompanyExamQuestionDTOs
{
    public class EditCompanyExamQuestionDTO
    {
        [Required(ErrorMessage = "Exam ID is required")]
        [Display(Name = "Exam ID", Description = "Reference to the parent exam")]
        [Range(1, int.MaxValue, ErrorMessage = "Exam ID must be a positive number")]
        public int ExamId { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        [Display(Name = "Question Text", Description = "The main question content")]
        [StringLength(4000, MinimumLength = 10, ErrorMessage = "Question text must be between 10 and 4,000 characters")]
        public string QuestionText { get; set; } = string.Empty;

        [Required(ErrorMessage = "Question type is required")]
        [Display(Name = "Question Type", Description = "Type of question (Multiple Choice, Written, etc.)")]
        [EnumDataType(typeof(QuestionType), ErrorMessage = "Invalid question type")]
        public QuestionType QuestionType { get; set; }

        [Required(ErrorMessage = "Points value is required")]
        [Range(0.01, 999.99, ErrorMessage = "Points must be between 0.01 and 999.99")]
        [Display(Name = "Points", Description = "Points awarded for correct answer")]
        public decimal Points { get; set; }

        [Display(Name = "Explanation", Description = "Optional explanation for the correct answer")]
        [StringLength(2000, ErrorMessage = "Explanation cannot exceed 2,000 characters")]
        public string? Explanation { get; set; }

        [Display(Name = "Code Snippet", Description = "Code example for programming questions")]
        [StringLength(2000, ErrorMessage = "Code snippet cannot exceed 2,000 characters")]
        public string? CodeSnippet { get; set; }

        [Display(Name = "Expected Output", Description = "Expected output for code execution questions")]
        [StringLength(2000, ErrorMessage = "Expected output cannot exceed 2,000 characters")]
        public string? ExpectedOutput { get; set; }

        [Required]
        [Display(Name = "Is Required", Description = "Whether this question must be answered")]
        public bool IsRequired { get; set; } = false;

        public void UpdateEntity(CompanyExamQuestion question)
        {
            question.ExamId = ExamId;
            question.QuestionText = QuestionText;
            question.QuestionType = QuestionType;
            question.Points = Points;
            question.Explanation = Explanation;
            question.CodeSnippet = CodeSnippet;
            question.ExpectedOutput = ExpectedOutput;
            question.IsRequired = IsRequired;
        }
    }
}