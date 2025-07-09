namespace Fit4Job.ViewModels.TrackAttemptsViewModels
{
    public class TrackAttemptSummaryViewModel
    {

        [Display(Name = "Attempt ID")]
        public int Id { get; set; }

        [Display(Name = "Track ID")]
        public int TrackId { get; set; }

        [Display(Name = "Track Name")]
        public string TrackName { get; set; } = string.Empty;

        [Display(Name = "Total Score")]
        public decimal TotalScore { get; set; }

        [Display(Name = "Progress")]
        public int ProgressPercentage { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = string.Empty;

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        public TrackAttemptSummaryViewModel()
        {
        }

        public TrackAttemptSummaryViewModel(TrackAttempt attempt)
        {
            Id = attempt.Id;
            TrackId = attempt.TrackId;
            TrackName = attempt.Track?.Name ?? "";
            TotalScore = attempt.TotalScore;
            ProgressPercentage = attempt.ProgressPercentage;
            Status = attempt.Status.ToString();
            StartTime = attempt.StartTime;
        }

        public static TrackAttemptSummaryViewModel GetViewModel(TrackAttempt attempt)
        {
            return new TrackAttemptSummaryViewModel(attempt);
        }
    }
}
