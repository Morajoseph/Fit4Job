using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fit4Job.Models
{
    public class CompanyProfile
    {
        [Key]
        public int CompanyId { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Company first name is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Company Name must be between 3 and 200 characters.")]
        public string CompanyName { get; set; }

        public string? CompanyDescription { get; set; }



        //  i can not find json data type here !!!!!!!!!!!!!!! 
        public string SocialLinks { get; set; }

        public string WebsiteUrl { get; set; }

        public string? Industry { get; set; }
        
        public string Location { get; set; }

        public enum CompanySize
        {
            startup,
            _1_10,
            _11_50,
            _51_200,
            _201_500,
            _500plus
        }

        public CompanySize? Company_Size { get; set; }


        public int FoundingYear { get; set; }

        public enum CompanyStatus
        {
            pending,
            approved,
            rejected
        }

        [Required]
        public CompanyStatus Status  { get; set; } = CompanyStatus.pending;


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


        [ForeignKey("User")]
        public int UserId { get; set; }
       public ApplicationUser User { get; set; }
    }
}
