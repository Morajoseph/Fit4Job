namespace Fit4Job.ViewModels.TrackQuestionOptionsViewModels
{
    public class TrackQuestionOptionViewModel
    {


        [Display(Name = "Option ID")]
        public int Id { get; set; }

        [Display(Name = "Question ID")]
        public int QuestionId { get; set; }

        [Display(Name = "Option Text")]
        public string OptionText { get; set; } = string.Empty;

        [Display(Name = "Is Correct")]
        public bool IsCorrect { get; set; }

        public TrackQuestionOptionViewModel() { }

        public TrackQuestionOptionViewModel(TrackQuestionOption option)
        {
            Id = option.Id;
            QuestionId = option.QuestionId;
            OptionText = option.OptionText;
            IsCorrect = option.IsCorrect;
        }

        public static TrackQuestionOptionViewModel GetViewModel(TrackQuestionOption option)
        {
            return new TrackQuestionOptionViewModel(option);
        }
    }
}
