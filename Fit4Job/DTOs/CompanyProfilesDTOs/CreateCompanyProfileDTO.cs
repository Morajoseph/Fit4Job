
namespace Fit4Job.DTOs.CompanyProfileDTOs
{
    public class CreateCompanyProfileDTO
    {
        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID", Description = "Reference to the associated user account")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive number")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 255 characters")]
        [Display(Name = "Company Name", Description = "Official name of the company")]
        public string CompanyName { get; set; } = string.Empty;

        [Display(Name = "Company Description", Description = "Detailed description of the company and its services")]
        [StringLength(2000, ErrorMessage = "Company description cannot exceed 2,000 characters")]
        [DataType(DataType.MultilineText)]
        public string? CompanyDescription { get; set; }

        [Url(ErrorMessage = "Please enter a valid LinkedIn URL")]
        [StringLength(255, ErrorMessage = "LinkedIn URL cannot exceed 255 characters")]
        [Display(Name = "LinkedIn URL", Description = "Company's LinkedIn profile URL")]
        public string? LinkedinUrl { get; set; }

        [Url(ErrorMessage = "Please enter a valid website URL")]
        [StringLength(255, ErrorMessage = "Website URL cannot exceed 255 characters")]
        [Display(Name = "Website URL", Description = "Company's official website URL")]
        public string? WebsiteUrl { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Industry must be between 2 and 100 characters")]
        [Display(Name = "Industry", Description = "Primary industry or sector of the company")]
        public string? Industry { get; set; }

        [Display(Name = "Company Size", Description = "Number of employees in the company")]
        [EnumDataType(typeof(CompanySize), ErrorMessage = "Invalid company size")]
        public CompanySize? CompanySize { get; set; }

        [Range(1800, 3000, ErrorMessage = "Founding year must be between 1800 and 3000")]
        [Display(Name = "Founding Year", Description = "Year when the company was founded")]
        public int? FoundingYear { get; set; }

        public CompanyProfile ToEntity()
        {
            return new CompanyProfile()
            {
                UserId = this.UserId,
                CompanyName = this.CompanyName,
                CompanyDescription = this.CompanyDescription,
                LinkedinUrl = this.LinkedinUrl,
                WebsiteUrl = this.WebsiteUrl,
                Industry = this.Industry,
                CompanySize = this.CompanySize,
                FoundingYear = this.FoundingYear,
                Status = CompanyStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}