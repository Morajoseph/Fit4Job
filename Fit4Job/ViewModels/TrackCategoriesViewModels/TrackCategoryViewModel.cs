namespace Fit4Job.ViewModels.TrackCategoriesViewModels
{
    public class TrackCategoryViewModel
    {
        [Display(Name = "Track Category ID")]
        public int Id { get; set; }

        [Display(Name = "Track Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Category Description")]
        public string? Description { get; set; }

        public TrackCategoryViewModel()
        {

        }
        public TrackCategoryViewModel(TrackCategory category)
        {
            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
        }
        public static TrackCategoryViewModel GetViewModel(TrackCategory category)
        {
            return new TrackCategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };
        }
    }
}
