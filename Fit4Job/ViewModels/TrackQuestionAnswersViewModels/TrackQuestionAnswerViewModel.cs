namespace Fit4Job.ViewModels.TrackQuestionAnswersViewModels
{
    public class TrackQuestionAnswerViewModel
    {
        [Display(Name = "Answer ID")]
        public int Id { get; set; }

        [Display(Name = "Attempt ID")]
        public int AttemptId { get; set; }

        [Display(Name = "Question ID")]
        public int QuestionId { get; set; }

        [Display(Name = "Selected Options")]
        public List<int>? SelectedOptions { get; set; }

        [Display(Name = "Text Answer")]
        public string? TextAnswer { get; set; }

        [Display(Name = "Is Correct")]
        public bool IsCorrect { get; set; } = false;

        [Display(Name = "Points Earned")]
        public decimal PointsEarned { get; set; } = 0.00m;

        [Display(Name = "Answered At")]
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;

        public TrackQuestionAnswerViewModel()
        {

        }

        public TrackQuestionAnswerViewModel(TrackQuestionAnswer questionAnswer)
        {
            Id = questionAnswer.Id;
            AttemptId = questionAnswer.AttemptId;
            IsCorrect = questionAnswer.IsCorrect;
            QuestionId = questionAnswer.QuestionId;
            AnsweredAt = questionAnswer.AnsweredAt;
            TextAnswer = questionAnswer.TextAnswer;
            PointsEarned = questionAnswer.PointsEarned;
            SelectedOptions = GetSelectedOptions(questionAnswer.SelectedOptionsJson);
        }

        public static TrackQuestionAnswerViewModel GetViewModel(TrackQuestionAnswer questionAnswer)
        {
            return new TrackQuestionAnswerViewModel(questionAnswer);
        }

        public static List<int> GetSelectedOptions(string? SelectedOptionsJson)
        {
            if (string.IsNullOrEmpty(SelectedOptionsJson)) return new List<int>();

            try
            {
                return JsonSerializer.Deserialize<List<int>>(SelectedOptionsJson) ?? new List<int>();
            }
            catch
            {
                return new List<int>();
            }
        }
        public static string? GetSelectedOptionsJson(List<int> value)
        {
            var SelectedOptionsJson = value?.Count > 0 ? JsonSerializer.Serialize(value) : null;
            return SelectedOptionsJson;
        }
    }
}
