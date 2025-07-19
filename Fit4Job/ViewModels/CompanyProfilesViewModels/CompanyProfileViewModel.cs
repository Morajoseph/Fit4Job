namespace Fit4Job.ViewModels.CompanyProfileViewModels
{
    public class CompanyProfileViewModel
    {
        [Display(Name = "Company ID")]
        public int Id { get; set; }

        [Display(Name = "User ID", Description = "Reference to the associated user account")]
        public int UserId { get; set; }

        [Display(Name = "Profile Picture URL")]
        public string? ProfilePictureURL { get; set; }

        [Display(Name = "Cover Picture URL")]
        public string? CoverPictureURL { get; set; }

        [Display(Name = "Bio")]
        public string? Bio { get; set; }

        [Display(Name = "Role")]
        public UserRole Role { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Company Name", Description = "Official name of the company")]
        public string CompanyName { get; set; } = string.Empty;

        [Display(Name = "Company Description", Description = "Detailed description of the company and its services")]
        public string? CompanyDescription { get; set; }

        [Display(Name = "LinkedIn URL", Description = "Company's LinkedIn profile URL")]
        public string? LinkedinUrl { get; set; }

        [Display(Name = "Website URL", Description = "Company's official website URL")]
        public string? WebsiteUrl { get; set; }

        [Display(Name = "Industry", Description = "Primary industry or sector of the company")]
        public string? Industry { get; set; }

        [Display(Name = "Company Size", Description = "Number of employees in the company")]
        public CompanySize? CompanySize { get; set; }

        [Display(Name = "Founding Year", Description = "Year when the company was founded")]
        public int? FoundingYear { get; set; }

        [Display(Name = "Status", Description = "Current approval status of the company profile")]
        public CompanyStatus Status { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Number Of Jobs")]
        public int NumberOfJobs { get; set; } = 0;

        public CompanyProfileViewModel()
        {

        }

        public CompanyProfileViewModel(CompanyProfile companyProfile)
        {
            Id = companyProfile.Id;
            UserId = companyProfile.UserId;
            CompanyName = companyProfile.CompanyName;
            CompanyDescription = companyProfile.CompanyDescription;
            LinkedinUrl = companyProfile.LinkedinUrl;
            WebsiteUrl = companyProfile.WebsiteUrl;
            Industry = companyProfile.Industry;
            CompanySize = companyProfile.CompanySize;
            FoundingYear = companyProfile.FoundingYear;
            Status = companyProfile.Status;
            ProfilePictureURL = companyProfile.User?.ProfilePictureURL;
            CoverPictureURL = companyProfile.User?.CoverPictureURL;
            Bio = companyProfile.User?.Bio;
            Role = companyProfile.User?.Role ?? UserRole.Company; 
            Email = companyProfile.User?.Email ?? string.Empty;
            NumberOfJobs = companyProfile.Jobs?.Count ?? 0;
            CreatedAt = companyProfile.CreatedAt;
        }

        public static CompanyProfileViewModel GetViewModel(CompanyProfile companyProfile)
        {
            return new CompanyProfileViewModel(companyProfile);
        }
    }
}