namespace Fit4Job.Models
{
    [Table("user_skills")]
    [Index(nameof(UserId), nameof(SkillId), IsUnique = true, Name = "IX_UserSkills_UserId_SkillId")]
    public class UserSkill
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public int UserId { get; set; }


        [Required(ErrorMessage = "Skill ID is required")]
        [Display(Name = "Skill ID")]
        public int SkillId { get; set; }


        [Required(ErrorMessage = "Created At timestamp is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }

        // Computed property
        [NotMapped]
        [Display(Name = "Is Active")]
        public bool IsActive => DeletedAt == null;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey("SkillId")]
        public virtual Skill Skill { get; set; } = null!;
    }
}
