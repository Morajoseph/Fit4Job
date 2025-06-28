using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fit4Job.Models
{
    public class JobSeekerProfile
    {
        [Key]
        public  int ProfileId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "User first name is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "First Name must be between 3 and 20 characters.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "User last name is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Last Name must be between 3 and 20 characters.")]
        public string LastName { get; set; }


        public string CvUrl { get; set; }

        public string LinkedinUrl { get; set; }

        public string GithubUrl { get; set; }
        
        public string? PortfolioUrl { get; set; }

        public int ExperienceYears { get; set; } = 0;

        public string CurrentPosition { get; set; }

        [Required]
        public double ExpectedSalary { get; set; }

        public int UserCredit { get; set; } = 5;
        public string Location {  get; set; }


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
