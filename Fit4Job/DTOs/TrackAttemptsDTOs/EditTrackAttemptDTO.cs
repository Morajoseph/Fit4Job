namespace Fit4Job.DTOs.TrackAttemptsDTOs
{
    public class EditTrackAttemptDTO
    {

        public int AttemptId { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Track ID is required")]
        public int TrackId { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public AttemptStatus Status { get; set; }


        [Required]
        [Range(0, 99999999.99, ErrorMessage = "Total score must be between 0 and 99,999,999.99")]
        [Display(Name = "Total Score")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalScore { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Progress percentage must be between 0 and 100")]
        [Display(Name = "Progress Percentage")]
        [DisplayFormat(DataFormatString = "{0}%", ApplyFormatInEditMode = false)]
        public int ProgressPercentage { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Solved Questions Count")]
        public int SolvedQuestionsCount { get; set; }

        public EditTrackAttemptDTO() { }

        public EditTrackAttemptDTO(TrackAttempt attempt)
        {
            AttemptId = attempt.Id;
            UserId = attempt.UserId;
            TrackId = attempt.TrackId;
            Status = attempt.Status;
            TotalScore = attempt.TotalScore;
            ProgressPercentage = attempt.ProgressPercentage;
            SolvedQuestionsCount = attempt.SolvedQuestionsCount;

        }




    }
}
