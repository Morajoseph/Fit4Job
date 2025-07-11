namespace Fit4Job.DTOs.TrackQuestionAnswersDTOs
{
    public class TrackQuestionAnswerDTO
    {
        [Display(Name = "Attempt ID")]
        [Required(ErrorMessage = "Attempt ID is required")]
        public int AttemptId { get; set; }

        [Display(Name = "Question ID")]
        [Required(ErrorMessage = "Question ID is required")]
        public int QuestionId { get; set; }

        [Display(Name = "Selected Options")]
        public List<int>? SelectedOptions { get; set; }

        [Display(Name = "Text Answer")]
        [MaxLength(2000, ErrorMessage = "Text answer cannot exceed 2,000 characters")]
        public string? TextAnswer { get; set; }

        public TrackQuestionAnswer GetModel()
        {
            return new TrackQuestionAnswer()
            {
                AttemptId = this.AttemptId,
                QuestionId = this.QuestionId,
                TextAnswer = this.TextAnswer,
                AnsweredAt = DateTime.UtcNow,
                SelectedOptionsJson = GetSelectedOptionsJson()
            };
        }

        public string? GetSelectedOptionsJson()
        {
            var SelectedOptionsJson = SelectedOptions?.Count > 0 ? JsonSerializer.Serialize(SelectedOptions) : null;
            return SelectedOptionsJson;
        }
    }
}