namespace Fit4Job.DTOs.CompanyExamQuestionOptionsDTOs
{
    public class CreateCompanyExamQuestionOptionDTO
    {
        [Required(ErrorMessage = "Question ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid question")]
        [Display(Name = "Question ID")]
        public int QuestionId { get; set; }

        [Display(Name = "Option Text")]
        [Required(ErrorMessage = "Option text is required")]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Option text must be between 1 and 2000 characters")]
        public string OptionText { get; set; } = string.Empty;

        [Display(Name = "Is Correct")]
        [Required(ErrorMessage = "You must specify whether this option is correct")]
        public bool IsCorrect { get; set; }

        public CompanyExamQuestionOption ToModel()
        {
            return new CompanyExamQuestionOption
            {
                QuestionId = this.QuestionId,
                OptionText = this.OptionText,
                IsCorrect = this.IsCorrect,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
