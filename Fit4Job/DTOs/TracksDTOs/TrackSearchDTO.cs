namespace Fit4Job.DTOs.TracksDTOs
{
    public class TrackSearchDTO
    {
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public int? CreatorId { get; set; }
        public bool? IsActive { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
