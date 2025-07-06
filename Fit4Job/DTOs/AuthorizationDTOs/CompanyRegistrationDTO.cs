namespace Fit4Job.DTOs.AuthorizationDTOs
{
    public class CompanyRegistrationDTO : BaseRegistrationDTO
    {
        [Required(ErrorMessage = "Company name is required")]
        [Display(Name = "Company Name", Description = "Official name of the company")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 255 characters")]
        public string CompanyName { get; set; } = string.Empty;


        [EnumDataType(typeof(CompanySize), ErrorMessage = "Invalid company size")]
        [Display(Name = "Company Size", Description = "Number of employees in the company")]
        public CompanySize? CompanySize { get; set; }
    }
}
