using Fit4Job.ViewModels.ComplexViewModels;

namespace Fit4Job.ViewModels.TracksViewModels
{
    public class TrackViewModel
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

        public TrackQuestionsDetailsViewModel TrackDetails { get; set; }

        public TrackViewModel()
        {

        }
        public TrackViewModel (Track track)
        {
            this.Id = track.Id;
            this.CategoryId = track.CategoryId;
            this.Name = track.Name;
            this.Description = track.Description;
            this.IsPremium = track.IsPremium;
            this.Price = track.Price;
            this.TrackQuestionsCount = track.TrackQuestionsCount;
            this.TrackTotalScore = track.TrackTotalScore;
        }
        public static TrackViewModel GetViewModel(Track track)
        {
            return new TrackViewModel()
            {
                Id = track.Id,
                CategoryId = track.CategoryId,
                Name = track.Name,
                Description = track.Description,
                IsPremium = track.IsPremium,
                Price = track.Price,
                TrackQuestionsCount = track.TrackQuestionsCount,
                TrackTotalScore = track.TrackTotalScore,
            }; 
        }
    }
}
