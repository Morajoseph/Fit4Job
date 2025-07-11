namespace Fit4Job.DTOs.TrackAttemptsDTOs
{
    public class CreateTrackAttemptDTO
    {

        [Required(ErrorMessage = "User is required")]
        [Display(Name = "User ID")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid user")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Track is required")]
        [Display(Name = "Track ID")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid track")]
        public int TrackId { get; set; }

        public TrackAttempt GetTrack()
        {
            return new TrackAttempt()
            {
                UserId = this.UserId,
                TrackId = this.TrackId
            };
        }

    }
}
