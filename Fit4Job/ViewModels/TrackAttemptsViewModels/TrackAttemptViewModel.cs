namespace Fit4Job.ViewModels.TrackAttemptsViewModels
{
    public class TrackAttemptViewModel
    {
        [Display(Name = "Attempt ID")]
        public int Id { get; set; }

        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Display(Name = "Track ID")]
        public int TrackId { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        [Display(Name = "End Time")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "Total Score")]
        public decimal TotalScore { get; set; } = 0.00m;

        [Display(Name = "Progress Percentage")]
        public int ProgressPercentage { get; set; } = 0;

        [Display(Name = "Status")]
        [EnumDataType(typeof(AttemptStatus))]
        public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;

        [Display(Name = "Solved Questions Count")]
        public int SolvedQuestionsCount { get; set; } = 0;

        public TrackAttemptViewModel()
        {

        }
        public TrackAttemptViewModel(TrackAttempt trackAttempt)
        {
            Id = trackAttempt.Id;
            UserId = trackAttempt.UserId;
            TrackId = trackAttempt.TrackId;
            StartTime = trackAttempt.StartTime;
            EndTime = trackAttempt.EndTime;
            TotalScore = trackAttempt.TotalScore;
            ProgressPercentage = trackAttempt.ProgressPercentage;
            Status = trackAttempt.Status;
            SolvedQuestionsCount = trackAttempt.SolvedQuestionsCount;
        }

        public static TrackAttemptViewModel GetViewModel(TrackAttempt trackAttempt)
        {
            return new TrackAttemptViewModel()
            {
                Id = trackAttempt.Id,
                UserId = trackAttempt.UserId,
                TrackId = trackAttempt.TrackId,
                StartTime = trackAttempt.StartTime,
                EndTime = trackAttempt.EndTime,
                TotalScore = trackAttempt.TotalScore,   
                ProgressPercentage = trackAttempt.ProgressPercentage,
                Status = trackAttempt.Status,
                SolvedQuestionsCount = trackAttempt.SolvedQuestionsCount,
            };
        }
    }
}
