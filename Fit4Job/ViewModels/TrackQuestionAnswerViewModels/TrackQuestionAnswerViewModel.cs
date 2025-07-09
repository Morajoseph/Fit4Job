namespace Fit4Job.ViewModels.TrackQuestionAnswerViewModel
{
    public class TrackQuestionAnswerViewModel
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public bool IsCorrect { get; set; }

        public decimal PointsEarned { get; set; }

        public DateTime AnsweredAt { get; set; }

        public string? TextAnswer { get; set; }

        public List<int> SelectedOptions { get; set; } = new();

        public string AnswerType { get; set; } = string.Empty;

        public static TrackQuestionAnswerViewModel FromModel(TrackQuestionAnswer answer)
        {
            return new TrackQuestionAnswerViewModel
            {
                Id = answer.Id,
                QuestionId = answer.QuestionId,
                IsCorrect = answer.IsCorrect,
                PointsEarned = answer.PointsEarned,
                AnsweredAt = answer.AnsweredAt,
                TextAnswer = answer.TextAnswer,
                SelectedOptions = answer.SelectedOptions,
                AnswerType = answer.AnswerType
            };
        }
    }
}
