namespace Fit4Job.DTOs.TrackQuestionAnswersDTOs
{
    public class EditTrackQuestionAnswerDTO
    {

        [Display(Name = "Selected Options")]
        public List<int>? SelectedOptions { get; set; }

        [Display(Name = "Text Answer")]
        [MaxLength(2000, ErrorMessage = "Text answer cannot exceed 2,000 characters")]
        public string? TextAnswer { get; set; }

        public void UpdateEntity(TrackQuestionAnswer questionAnswer)
        {
            questionAnswer.TextAnswer = TextAnswer;
            if (SelectedOptions != null)
            {
                questionAnswer.SelectedOptions = SelectedOptions;
                questionAnswer.SelectedOptionsJson = GetSelectedOptionsJson();
            }
            questionAnswer.AnsweredAt = DateTime.UtcNow;
        }

        public string? GetSelectedOptionsJson()
        {
            var SelectedOptionsJson = SelectedOptions?.Count > 0 ? JsonSerializer.Serialize(SelectedOptions) : null;
            return SelectedOptionsJson;
        }
    }
}
