namespace Fit4Job.DTOs.TracksDTOs
{
    public class EditTrackDTO
    {
        [Display(Name = "Track Category ID")]
        [Range(0, int.MaxValue, ErrorMessage = "Please select a valid category")]
        public int CategoryId { get; set; }

        [StringLength(256)]
        [Display(Name = "Track Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        [Display(Name = "Track Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Is Premium")]
        public bool IsPremium { get; set; } = false;

        [Range(0, 99999999.99)]
        [Display(Name = "Price")]
        public decimal? Price { get; set; } = 0;

        public EditTrackDTO()
        {

        }

        public void UpdateEntity(Track track)
        {
            if (CategoryId != 0)
            {
                track.CategoryId = CategoryId;
            }
            if (Name != null && Name != string.Empty)
            {
                track.Name = Name;
            }
            if(IsPremium)
            {
                track.IsPremium = true;
                track.Price = Price;
            }
            track.UpdatedAt = DateTime.UtcNow;
        }
        public EditTrackDTO(Track track)
        {
            CategoryId = track.CategoryId;
            Name = track.Name;
            Description = track.Description;
            IsPremium = track.IsPremium;
            Price = track.Price;
        }
    }
}
