using System.ComponentModel.DataAnnotations;

namespace Fit4Job.ViewModels.CompanyExamQuestionViewModels
{
    public class CompanyExamQuestionViewModel
    {
        [Display(Name = "Question ID")]
        public int Id { get; set; }

        [Display(Name = "Exam ID", Description = "Reference to the parent exam")]
        public int ExamId { get; set; }

        [Display(Name = "Question Text", Description = "The main question content")]
        public string QuestionText { get; set; } = string.Empty;

        [Display(Name = "Question Type", Description = "Type of question (Multiple Choice, Written, etc.)")]
        public QuestionType QuestionType { get; set; }

        [Display(Name = "Points", Description = "Points awarded for correct answer")]
        public decimal Points { get; set; }

        [Display(Name = "Explanation", Description = "Optional explanation for the correct answer")]
        public string? Explanation { get; set; }

        [Display(Name = "Code Snippet", Description = "Code example for programming questions")]
        public string? CodeSnippet { get; set; }

        [Display(Name = "Expected Output", Description = "Expected output for code execution questions")]
        public string? ExpectedOutput { get; set; }

        [Display(Name = "Is Required", Description = "Whether this question must be answered")]
        public bool IsRequired { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }

        // Helper properties
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Multiple Choice")]
        public bool IsMultipleChoice { get; set; }

        [Display(Name = "Is Text Based")]
        public bool IsTextBased { get; set; }

        public CompanyExamQuestionViewModel()
        {
        }

        public CompanyExamQuestionViewModel(CompanyExamQuestion question)
        {
            Id = question.Id;
            ExamId = question.ExamId;
            QuestionText = question.QuestionText;
            QuestionType = question.QuestionType;
            Points = question.Points;
            Explanation = question.Explanation;
            CodeSnippet = question.CodeSnippet;
            ExpectedOutput = question.ExpectedOutput;
            IsRequired = question.IsRequired;
            CreatedAt = question.CreatedAt;
            DeletedAt = question.DeletedAt;

            // Map helper properties
            IsActive = question.IsActive;
            IsMultipleChoice = question.IsMultipleChoice;
            IsTextBased = question.IsTextBased;
        }

        public static CompanyExamQuestionViewModel GetViewModel(CompanyExamQuestion question)
        {
            return new CompanyExamQuestionViewModel(question);
        }
    }
}