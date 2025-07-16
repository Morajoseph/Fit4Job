
namespace Fit4Job.ViewModels.CompanyProfileViewModels
{
    public class CompanyProfileViewModel
    {
        [Display(Name = "Company ID")]
        public int Id { get; set; }

        [Display(Name = "User ID", Description = "Reference to the associated user account")]
        public int UserId { get; set; }

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

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }

        // Helper properties
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; }

        [Display(Name = "Is Pending")]
        public bool IsPending { get; set; }

        [Display(Name = "Company Age")]
        public int? CompanyAge { get; set; }

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
            CreatedAt = companyProfile.CreatedAt;
            UpdatedAt = companyProfile.UpdatedAt;
            DeletedAt = companyProfile.DeletedAt;

            // Map helper properties
            IsActive = companyProfile.IsActive;
            IsApproved = companyProfile.IsApproved;
            IsPending = companyProfile.IsPending;
            CompanyAge = companyProfile.CompanyAge;
        }

        public static CompanyProfileViewModel GetViewModel(CompanyProfile companyProfile)
        {
            return new CompanyProfileViewModel(companyProfile);
        }
    }
}