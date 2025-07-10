using Fit4Job.ViewModels.TrackQuestionOptionsViewModels;
using Fit4Job.ViewModels.TracksViewModels;

namespace Fit4Job.ViewModels.ComplexViewModels
{
    public class TrackQuestionWithOptionsViewModel
    {
        public TrackQuestionViewModel Question { get; set; } = null!;
        public IEnumerable<TrackQuestionOptionViewModel> Options { get; set; } = null!;
        
        public TrackQuestionWithOptionsViewModel()
        {

        }

        public TrackQuestionWithOptionsViewModel(TrackQuestionViewModel question, IEnumerable<TrackQuestionOptionViewModel> options)
        {
            Question = question;
            Options = options;
        }

        public static TrackQuestionWithOptionsViewModel GetViewModel(TrackQuestionViewModel question, IEnumerable<TrackQuestionOptionViewModel> options)
        {
            return new TrackQuestionWithOptionsViewModel(question, options);
        }
    }
}