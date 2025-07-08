namespace Fit4Job.DTOs.TrackAttemptsDTOs
{
    public class CreateTrackAttemptDTO
    {
        [Required(ErrorMessage = "Track is required")]
        [Display(Name = "Track ID")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid track")]
        public int TrackId { get; set; }




        public TrackAttempt GetTrack(int userId)
        {
            return new TrackAttempt()
            {
                UserId = userId,
                TrackId = TrackId
            };
        }

    }
}
