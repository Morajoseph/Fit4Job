namespace Fit4Job.DTOs.TracksDTOs
{
    public class EditTrackDTO
    {

        public int TrackId { get; set; }

        [Required(ErrorMessage = "Track Category is required")]
        [Display(Name = "Track Category ID")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Track name is required")]
        [StringLength(256)]
        [Display(Name = "Track Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        [Display(Name = "Track Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Is Premium")]
        public bool IsPremium { get; set; } = false;

        [Required]
        [Range(0, 99999999.99)]
        [Display(Name = "Price")]
        public decimal? Price { get; set; } = 0;

        [Required]
        [Display(Name = "Track Questions")]
        public int TrackQuestionsCount { get; set; } = 0;

    
        [Required]
        [Range(0, 99999999.99)]
        [Display(Name = "Track Total Score")]
        public decimal TrackTotalScore { get; set; }

        public EditTrackDTO() { }


        public EditTrackDTO(Track track)
            {
            TrackId = track.Id;
            CategoryId = track.CategoryId;
            Name = track.Name;
            Description = track.Description;
            IsPremium = track.IsPremium;
            Price = track.Price;
            TrackQuestionsCount = track.TrackQuestionsCount;
            TrackTotalScore = track.TrackTotalScore;
        
            
            }
    }
}
