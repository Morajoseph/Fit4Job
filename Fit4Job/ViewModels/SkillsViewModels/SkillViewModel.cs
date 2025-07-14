namespace Fit4Job.ViewModels.SkillsViewModels
{
    public class SkillViewModel
    {
        [Display(Name = "Skill ID")]
        public int Id { get; set; }

        [Display(Name = "Skill")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        public SkillViewModel()
        {

        }

        public SkillViewModel(Skill skill)
        {
            Id = skill.Id;
            Name = skill.Name;
            Description = skill.Description;
            IsActive = skill.IsActive;
        }

        public static SkillViewModel GetViewModel(Skill skill)
        {
            return new SkillViewModel(skill);
        }
    }
}
