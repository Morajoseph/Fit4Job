namespace Fit4Job.DTOs.AuthorizationDTOs
{
    public class VerificationDTO
    {
        [Display(Name = "Email or Username")]
        [Required(ErrorMessage = "Email or Username is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Email or Username must be between 3 and 100 characters.")]
        public string EmailOrUsername { get; set; } = string.Empty;

        [Display(Name = "Verification Code", Description = "Verification Code Email")]
        [Required(ErrorMessage = "Verification Code is required")]
        public string VerificationCode { get; set; } = string.Empty;
    }
}