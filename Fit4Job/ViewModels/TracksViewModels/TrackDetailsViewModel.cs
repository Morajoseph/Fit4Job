namespace Fit4Job.ViewModels.TracksViewModels
{
    public class TrackDetailsViewModel
    {
        [Display(Name = "Track ID")]
        public int Id { get; set; }

        [Display(Name = "Track Category ID")]
        public int CategoryId { get; set; }

        [Display(Name = "Track Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Track Description")]
        public string? Description { get; set; }

        [Display(Name = "Is Premium")]
        public bool IsPremium { get; set; } = false;

        [Display(Name = "Price")]
        public decimal? Price { get; set; } = 0;

        [Display(Name = "Track Questions")]
        public int TrackQuestionsCount { get; set; } = 0;

        [Display(Name = "Track Total Score")]
        public decimal TrackTotalScore { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = string.Empty;

        [Display(Name = "Category Description")]
        public string? CategoryDescription { get; set; }

        [Display(Name = "Creator Username")]
        public string CreatorUsername { get; set; } = string.Empty;

        [Display(Name = "Creator Email")]
        public string CreatorEmail { get; set; } = string.Empty;

        [Display(Name = "Creator Role")]
        public string CreatorRole { get; set; } = string.Empty;

        public TrackDetailsViewModel()
        {
        }

        public TrackDetailsViewModel(Track track)
        {
            Id = track.Id;
            CategoryId = track.CategoryId;
            Name = track.Name;
            Description = track.Description;
            IsPremium = track.IsPremium;
            Price = track.Price;
            TrackQuestionsCount = track.TrackQuestionsCount;
            TrackTotalScore = track.TrackTotalScore;
            CategoryName = track.Category?.Name ?? "";
            CategoryDescription = track.Category?.Description ?? "";
            CreatorUsername = track.Creator?.UserName ?? "";
            CreatorEmail = track.Creator?.Email ?? "";
            CreatorRole = track.Creator?.Role.ToString() ?? "";
        }

        public static TrackDetailsViewModel GetViewModel(Track track)
        {
            return new TrackDetailsViewModel(track);
        }
    }
}
