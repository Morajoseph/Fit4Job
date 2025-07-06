namespace Fit4Job.DTOs.AuthorizationDTOs
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "First name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 20 characters")]
        [Display(Name = "First Name")]

        public string FirstName { get; set; }




        [Required(ErrorMessage = "Last name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 20 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address!.")]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool RememberMe { get; set; }

    }
}
