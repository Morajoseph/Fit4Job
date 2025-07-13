namespace Fit4Job.ViewModels.TrackCategoriesViewModels
{
    public class TrackCategoryViewModel
    {
        [Display(Name = "Track Category ID")]
        public int Id { get; set; }

        [Display(Name = "Track Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Icon URL")]
        public string? IconUrl { get; set; }

        [Display(Name = "Category Description")]
        public string? Description { get; set; }

        [Display(Name = "Category Skills")]
        public List<string> CategorySkills { get; set; }

        public TrackCategoryViewModel()
        {

        }
        public TrackCategoryViewModel(TrackCategory category)
        {
            Id = category.Id;
            Name = category.Name;
            IconUrl = category.IconUrl;
            Description = category.Description;
            CategorySkills = category.CategorySkills;
        }
        public static TrackCategoryViewModel GetViewModel(TrackCategory category)
        {
            return new TrackCategoryViewModel(category);
        }
    }
}
