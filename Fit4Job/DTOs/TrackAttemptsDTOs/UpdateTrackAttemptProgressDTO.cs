namespace Fit4Job.DTOs.TrackAttemptsDTOs
{
    public class UpdateTrackAttemptProgressDTO
    {

        [Required]
        [Range(0, 100, ErrorMessage = "Progress percentage must be between 0 and 100")]
        [Display(Name = "Progress Percentage")]
        public int ProgressPercentage { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Solved questions count must be a positive number")]
        [Display(Name = "Solved Questions Count")]
        public int SolvedQuestionsCount { get; set; }


        public void UpdateAttempt(TrackAttempt attempt)
        {
            attempt.ProgressPercentage = this.ProgressPercentage;
            attempt.SolvedQuestionsCount = this.SolvedQuestionsCount;

            if (this.ProgressPercentage == 100)
            {
                attempt.Status = AttemptStatus.Completed;
                attempt.EndTime = DateTime.UtcNow;
            }


        }
    }
}
