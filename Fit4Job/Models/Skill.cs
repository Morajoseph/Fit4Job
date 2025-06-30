namespace Fit4Job.Models
{
    [Table("skills")]
    [Index(nameof(Name), IsUnique = true, Name = "IX_Skills_Name")]
    [Index(nameof(IsActive), Name = "IX_Skills_IsActive")]
    public class Skill
    {
        [Key]
        [Display(Name = "Skill ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Skill name is required")]
        [StringLength(255)]
        [Display(Name = "Skill Name")]
        public string Name { get; set; } = string.Empty;

      
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        [Column(TypeName = "text")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        [Display(Name = "User Skills")]
        public virtual ICollection<UserSkill> UserSkills { get; set; }
    }
}
