namespace Fit4Job.ViewModels.TrackQuestionsViewModels
{
    public class TrackQuestionViewModel
    {
        [Display(Name = "Question ID")]
        public int Id { get; set; }

        [Display(Name = "Track ID")]
        public int TrackId { get; set; }

        [Display(Name = "Question Text")]
        public string QuestionText { get; set; } = string.Empty;

        [Display(Name = "Question Type")]
        public string QuestionType { get; set; } = string.Empty;

        [Display(Name = "Question Level")]
        public string QuestionLevel { get; set; } = string.Empty;

        [Display(Name = "Difficulty Level")]
        public string DifficultyLevel { get; set; } = string.Empty;

        [Display(Name = "Points")]
        public decimal Points { get; set; }

        [Display(Name = "Explanation")]
        public string? Explanation { get; set; }

        [Display(Name = "Code Snippet")]
        public string? CodeSnippet { get; set; }

        [Display(Name = "Expected Output")]
        public string? ExpectedOutput { get; set; }

        public TrackQuestionViewModel()
        {

        }

        public TrackQuestionViewModel(TrackQuestion question)
        {
            Id = question.Id;
            TrackId = question.TrackId;
            QuestionText = question.QuestionText;
            QuestionType = question.QuestionType.ToString();
            QuestionLevel = question.QuestionLevel.ToString();
            DifficultyLevel = question.DifficultyLevel.ToString();
            Points = question.Points;
            Explanation = question.Explanation;
            CodeSnippet = question.CodeSnippet;
            ExpectedOutput = question.ExpectedOutput;
        }

        public static TrackQuestionViewModel GetViewModel(TrackQuestion question)
        {
            return new TrackQuestionViewModel(question);
        }
    }
}
