using System.ComponentModel.DataAnnotations;

namespace Fit4Job.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Skill Name")]
        [Required(ErrorMessage = "Skill  name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Skill Name must be between 2 and 50 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; } = true;


    }
}
