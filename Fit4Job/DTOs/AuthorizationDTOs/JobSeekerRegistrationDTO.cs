namespace Fit4Job.DTOs.AuthorizationDTOs
{
    public class JobSeekerRegistrationDTO : BaseRegistrationDTO
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 20 characters")]
        [Display(Name = "First Name", Description = "Administrator's first name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 20 characters")]
        [Display(Name = "Last Name", Description = "Administrator's last name")]
        public string LastName { get; set; } = string.Empty;

        public JobSeekerProfile ToEntity(int userId)
        {
            return new JobSeekerProfile()
            {
                UserId = userId,
                FirstName = this.FirstName,
                LastName = this.LastName,
            };
        }
    }
}
