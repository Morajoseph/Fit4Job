using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fit4Job.Models
{
    public class AdminProfile
    {
        [Key]
        public int  AdminId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "User first name is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "First Name must be between 3 and 20 characters.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "User last name is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Last Name must be between 3 and 20 characters.")]
        public string LastName { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        
        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }



        [ForeignKey("User")]
        public int UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
