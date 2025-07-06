namespace Fit4Job.DTOs.AuthorizationDTOs
{
    public class BaseRegistrationDTO
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address!.")]
        [Display(Name = "Email", Description = "Administrator's Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 20 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Username can only contain letters, numbers, underscores and hyphens.")]
        public string UserName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;


        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Confirm Password Does not match password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
