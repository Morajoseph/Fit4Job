namespace Fit4Job.Models
{
    [Table("user_skills")]
    [Index(nameof(UserId), nameof(SkillId), IsUnique = true, Name = "IX_UserSkills_UserId_SkillId")]
    [Index(nameof(DeletedAt), Name = "IX_UserSkills_DeletedAt")]
    public class UserSkill
    {
        [Key]
        [Required]
        [Column(Order = 0)]
        public int UserId { get; set; }

        [Key]
        [Required]
        [Column(Order = 1)]
        public int SkillId { get; set; }


        [Required]
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
