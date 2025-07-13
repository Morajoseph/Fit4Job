namespace Fit4Job.DTOs.TracksDTOs
{
    public class CreateTrackDTO
    {
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

        public Track ToEntity(int userId)
        {
            return new Track()
            {
                CreatorId = userId,
                CategoryId = this.CategoryId,
                Name = this.Name,
                Description = this.Description,
                IsPremium = this.IsPremium,
                Price = this.Price,
            };
        }
    }
}
