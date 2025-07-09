namespace Fit4Job.ViewModels.TracksViewModels
{
    public class BadgeViewModel
    {

        [Display(Name = "Badge ID")]
        public int Id { get; set; }

        [Display(Name = "Track ID")]
        public int TrackId { get; set; }

        [Display(Name = "Badge Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Icon URL")]
        public string? IconUrl { get; set; }

        [Display(Name = "Points Required")]
        public decimal PointsRequired { get; set; }

        public BadgeViewModel()
        {
        }

        public BadgeViewModel(Badge badge)
        {
            this.Id = badge.Id;
            this.TrackId = badge.TrackId;
            this.Name = badge.Name;
            this.Description = badge.Description;
            this.IconUrl = badge.IconUrl;
            this.PointsRequired = badge.PointsRequired;
        }

        public static BadgeViewModel GetViewModel(Badge badge)
        {
            return new BadgeViewModel(badge);
        }
    }
}
