namespace Fit4Job.DTOs.TrackQuestionOptionsDTOs
{
    public class CreateTrackQuestionOptionDTO
    {

        [Required(ErrorMessage = "Question ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid question")]
        [Display(Name = "Question ID")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Option text is required")]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Option text must be between 1 and 2000 characters")]
        [Display(Name = "Option Text")]
        public string OptionText { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must specify whether this option is correct")]
        [Display(Name = "Is Correct")]
        public bool IsCorrect { get; set; }

        public TrackQuestionOption ToModel()
        {
            return new TrackQuestionOption
            {
                QuestionId = this.QuestionId,
                OptionText = this.OptionText,
                IsCorrect = this.IsCorrect,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        }
}
