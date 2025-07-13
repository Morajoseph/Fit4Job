namespace Fit4Job.DTOs.TrackCategoriesDTOs
{
    public class CreateTrackCategoryDTO
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters")]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;

        [Url(ErrorMessage = "Please enter a valid URL")]
        [StringLength(500, ErrorMessage = "Icon URL cannot exceed 500 characters")]
        [Display(Name = "Icon URL")]
        public string? IconUrl { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        [Display(Name = "Category Description")]
        public string? Description { get; set; }

        [Display(Name = "Category Skills")]
        public List<int> CategorySkills { get; set; }

        public CreateTrackCategoryDTO()
        {

        }
        public CreateTrackCategoryDTO(TrackCategory category)
        {

            Name = category.Name;
            IconUrl = category.IconUrl;
            Description = category.Description;
            CategorySkills = category.CategorySkills;
        }
        public TrackCategory ToEntity()
        {
            return new TrackCategory
            {
                Name = this.Name,
                Description = this.Description,
                IconUrl = this.IconUrl,
                CategorySkills = this.CategorySkills
            };
        }
    }
}
