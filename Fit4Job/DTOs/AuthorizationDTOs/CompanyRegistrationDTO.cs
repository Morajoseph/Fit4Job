namespace Fit4Job.DTOs.AuthorizationDTOs
{
    public class CompanyRegistrationDTO : BaseRegistrationDTO
    {
        [Required(ErrorMessage = "Company name is required")]
        [Display(Name = "Company Name", Description = "Official name of the company")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 255 characters")]
        public string CompanyName { get; set; } = string.Empty;

        public CompanyProfile ToEntity(int userId)
        {
            return new CompanyProfile()
            {
                UserId = userId,
                CompanyName = CompanyName,
            };
        }
    }
}
