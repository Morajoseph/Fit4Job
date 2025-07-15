namespace Fit4Job.Services.Interfaces
{
    public interface IImageUploadService
    {
        Task<string?> UploadImageAsync(IFormFile file, string folder = "uploads");
        Task<bool> DeleteImageAsync(string imageUrl);
        bool IsValidImage(IFormFile file);
    }
}
