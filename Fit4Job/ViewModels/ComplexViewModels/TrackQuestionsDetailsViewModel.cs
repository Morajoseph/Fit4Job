namespace Fit4Job.ViewModels.ComplexViewModels
{
    public class TrackQuestionsDetailsViewModel
    {
        [Display(Name = "Track Easy Questions Counter")]
        public int EasyQuestionsCounter { get; set; } = 0;

        [Display(Name = "Track Medium Questions Counter")]
        public int MediumQuestionsCounter { get; set; } = 0;

        [Display(Name = "Track Hard Questions Counter")]
        public int HardQuestionsCounter { get; set; } = 0;


        [Display(Name = "Track True Or False Questions Counter")]
        public int TrueOrFalseQuestionsCounter { get; set; } = 0;

        [Display(Name = "Track Multiple Choice Questions Counter")]
        public int MultipleChoiceQuestionsCounter { get; set; } = 0;

        [Display(Name = "Track Written Questions Counter")]
        public int WrittenQuestionsCounter { get; set; } = 0;

        public TrackQuestionsDetailsViewModel()
        {

        }

        public TrackQuestionsDetailsViewModel(IEnumerable<TrackQuestion> questions)
        {
            EasyQuestionsCounter = questions.Where(q => q.DifficultyLevel == DifficultyLevel.Easy).Count();
            MediumQuestionsCounter = questions.Where(q => q.DifficultyLevel == DifficultyLevel.Medium).Count();
            HardQuestionsCounter = questions.Where(q => q.DifficultyLevel == DifficultyLevel.Hard).Count();

            TrueOrFalseQuestionsCounter = questions.Where(q => q.QuestionType == QuestionType.TrueFalse).Count();

            MultipleChoiceQuestionsCounter = questions.Where(q =>
            q.QuestionType == QuestionType.MultipleChoiceSingle ||
            q.QuestionType == QuestionType.MultipleChoiceMultiple).Count();

            WrittenQuestionsCounter = questions.Where(q =>
            q.QuestionType == QuestionType.Written ||
            q.QuestionType == QuestionType.CodeOutput ||
            q.QuestionType == QuestionType.FillInTheBlank).Count();
        }

        public static TrackQuestionsDetailsViewModel GetViewModel(IEnumerable<TrackQuestion> questions)
        {
            return new TrackQuestionsDetailsViewModel(questions);
        }

    }
}
