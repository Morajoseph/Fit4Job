namespace Fit4Job.ViewModels.CompanyExamQuestionOptionsViewModels
{
    public class CompanyExamQuestionOptionViewModel
    {
        [Display(Name = "Option ID")]
        public int Id { get; set; }

        [Display(Name = "Question ID")]
        public int QuestionId { get; set; }

        [Display(Name = "Option Text")]
        public string OptionText { get; set; } = string.Empty;

        public CompanyExamQuestionOptionViewModel()
        {

        }

        public CompanyExamQuestionOptionViewModel(CompanyExamQuestionOption option)
        {
            Id = option.Id;
            QuestionId = option.QuestionId;
            OptionText = option.OptionText;
        }

        public static CompanyExamQuestionOptionViewModel GetViewModel(CompanyExamQuestionOption option)
        {
            return new CompanyExamQuestionOptionViewModel(option);
        }
    }
}
