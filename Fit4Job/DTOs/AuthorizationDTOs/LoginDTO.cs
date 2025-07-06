namespace Fit4Job.DTOs.AuthorizationDTOs
{
    public class LoginDTO
    {
        [Display(Name = "Email or Username")]
        [Required(ErrorMessage = "Email or Username is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Email or Username must be between 3 and 100 characters.")]
        public string EmailOrUsername { get; set; } = string.Empty;

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me.")]
        public bool RememberMe { get; set; } = false;

    }
}
