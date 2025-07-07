namespace Fit4Job.DTOs.TrackCategoriesDTOs
{
    public class EditTrackCategoryDTO
    {
        [Display(Name = "Track Category ID")]
        public int Id { get; set; }

        [Display(Name = "Track Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Category Description")]
        public string? Description { get; set; }
        public EditTrackCategoryDTO(TrackCategory category) {

            Id = category.Id;
            Name = category.Name;
            Description = category.Description;

        }
        public EditTrackCategoryDTO() { }



    }
}
