namespace Fit4Job.DTOs.SkillsDTOs
{
    public class EditSkillDTO
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
        public bool IsActive { get; set; }

        public void UpdateEntity(Skill skill)
        {
            skill.Name = Name;
            skill.Description = Description;
            skill.IsActive = IsActive;
        }
    }
}
