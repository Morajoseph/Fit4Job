namespace Fit4Job.DTOs.TrackCategoriesDTOs
{
    public class CreateTrackCategoryDTO
    {
       

        [Display(Name = "Track Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Category Description")]
        public string? Description { get; set; }
        public CreateTrackCategoryDTO(TrackCategory category)
        {

            Name = category.Name;
            Description = category.Description;

        }
        public CreateTrackCategoryDTO() { }



    }
}
