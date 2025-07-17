namespace Fit4Job.DTOs.CompanyExamQuestionOptionsDTOs
{
    public class EditCompanyExamQuestionOptionDTO
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

        public void UpdateModel(CompanyExamQuestionOption questionOption)
        {
            questionOption.QuestionId = QuestionId;
            questionOption.IsCorrect = this.IsCorrect;
            questionOption.OptionText = this.OptionText;
        }
    }
}