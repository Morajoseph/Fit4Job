namespace Fit4Job.DTOs.SkillsDTOs
{
    public class CreateSkillDTO
    {
        [Required(ErrorMessage = "Skill name is required")]
        [StringLength(255)]
        [Display(Name = "Skill Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        public Skill ToEntity()
        {
            return new Skill()
            {
                Name = this.Name,
                Description = this.Description,
                IsActive = this.IsActive
            };
        }
    }
}
