namespace Fit4Job.DTOs.TrackQuestionsDTOs
{
    public class CreateTrackQuestionDTO
    {
        [Display(Name = "Track ID")]
        [Required(ErrorMessage = "Track is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid track")]
        public int TrackId { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Question text must be between 5 and 2,000 characters")]
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; } = string.Empty;

        [Required(ErrorMessage = "Question type is required")]
        [Display(Name = "Question Type")]
        [EnumDataType(typeof(QuestionType))]
        public QuestionType QuestionType { get; set; }

        [Required(ErrorMessage = "Question level is required")]
        [Display(Name = "Question Level")]
        [EnumDataType(typeof(QuestionLevel))]
        public QuestionLevel QuestionLevel { get; set; }

        [Required(ErrorMessage = "Difficulty level is required")]
        [Display(Name = "Difficulty Level")]
        [EnumDataType(typeof(DifficultyLevel), ErrorMessage = "Please select a valid difficulty level")]
        public DifficultyLevel DifficultyLevel { get; set; }

        [Required(ErrorMessage = "Points value is required")]
        [Range(0.01, 999.99, ErrorMessage = "Points must be between 0.01 and 999.99")]
        [Display(Name = "Points")]
        public decimal Points { get; set; } = 1;

        [StringLength(2000, ErrorMessage = "Explanation cannot exceed 2000 characters")]
        [Display(Name = "Explanation")]
        public string? Explanation { get; set; }

        [StringLength(2000, ErrorMessage = "Code snippet cannot exceed 2,000 characters")]
        [Column(TypeName = "nvarchar(2000)")]
        [Display(Name = "Code Snippet")]
        public string? CodeSnippet { get; set; }

        [StringLength(2000, ErrorMessage = "Expected output cannot exceed 2,000 characters")]
        [Column(TypeName = "nvarchar(2000)")]
        [Display(Name = "Expected Output")]
        public string? ExpectedOutput { get; set; }

        public TrackQuestion GetTrackQuestion()
        {
            return new TrackQuestion()
            {
                TrackId = this.TrackId,
                QuestionText = this.QuestionText,
                QuestionType = this.QuestionType,
                QuestionLevel = this.QuestionLevel,
                DifficultyLevel = this.DifficultyLevel,
                Explanation = this.Explanation,
                Points = this.Points,
                CodeSnippet = this.CodeSnippet,
                ExpectedOutput = this.ExpectedOutput
            };
        }
    }
}
