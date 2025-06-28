using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Fit4Job.Models
{
    public class ApplicationUser:IdentityUser<int>
    {


        [Display(Name = "First Name")]
        [Required(ErrorMessage = "User first name is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "First Name must be between 3 and 20 characters.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "User last name is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Last Name must be between 3 and 20 characters.")]
        public string LastName { get; set; }


        public string ProfilePictureUrl { get; set; }

        public string? Bio {  get; set; }



        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; } 
        public bool IsActive { get; set; } = true;
        public bool IsVerified { get; set; } = false;


        public enum UserRole
        {
            Admin,
            Company,
            JobSeeker
        }

        [Required]
        public UserRole Role { get; set; } = UserRole.JobSeeker;



    }
}
