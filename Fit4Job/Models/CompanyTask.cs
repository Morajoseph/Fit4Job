namespace Fit4Job.Models
{
    [Table("company_tasks")]
    [Index(nameof(CompanyId), Name = "IX_CompanyTasks_CompanyId")]
    [Index(nameof(IsActive), Name = "IX_CompanyTasks_IsActive")]
    [Index(nameof(Deadline), Name = "IX_CompanyTasks_Deadline")]
    public class CompanyTask
    {
        [Key]
        [Display(Name = "Task ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "text")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        [Display(Name = "Requirements")]
        public string? Requirements { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "Deliverables")]
        public string? Deliverables { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; }


        [Display(Name = "Estimated Hours")]
        [Range(1, int.MaxValue, ErrorMessage = "Estimated hours must be at least 1")]
        public int? EstimatedHours { get; set; }


        [Column(TypeName = "json")]
        [Display(Name = "Skills Required")]
        public string? SkillsRequiredJson { get; set; }


        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }

        // Computed properties for working with skills
        [NotMapped]
        [Display(Name = "Skills Required")]
        public List<string> SkillsRequired
        {
            get
            {
                if (string.IsNullOrEmpty(SkillsRequiredJson))
                    return new List<string>();

                try
                {
                    return JsonSerializer.Deserialize<List<string>>(SkillsRequiredJson) ?? new List<string>();
                }
                catch
                {
                    return new List<string>();
                }
            }
            set
            {
                SkillsRequiredJson = value?.Count > 0 ? JsonSerializer.Serialize(value) : null;
            }
        }

        // Computed properties
        [NotMapped]
        public bool IsDeleted => DeletedAt.HasValue;

        [NotMapped]
        [Display(Name = "Is Available")]
        public bool IsAvailable => IsActive && !IsDeleted && !IsExpired;

        [NotMapped]
        [Display(Name = "Is Expired")]
        public bool IsExpired => DateTime.UtcNow > Deadline;

        
        [NotMapped]
        [Display(Name = "Days Until Deadline")]
        public int DaysUntilDeadline => (Deadline.Date - DateTime.UtcNow.Date).Days;

        
        [NotMapped]
        [Display(Name = "Time Remaining")]
        public TimeSpan? TimeRemaining => IsExpired ? null : Deadline - DateTime.UtcNow;


        // Navigation properties
        [ForeignKey("CompanyId")]
        [Display(Name = "Company")]
        public virtual CompanyProfile Company { get; set; } = null!;

        public virtual ICollection<CompanyTaskSubmission>? Submissions { get; set; }

        // Helper methods
        //public bool RequiresSkill(string skill)
        //{
        //    return SkillsRequired.Any(s => s.Equals(skill, StringComparison.OrdinalIgnoreCase));
        //}

        //public void AddSkill(string skill)
        //{
        //    if (!string.IsNullOrWhiteSpace(skill))
        //    {
        //        var skills = SkillsRequired;
        //        if (!skills.Contains(skill, StringComparer.OrdinalIgnoreCase))
        //        {
        //            skills.Add(skill.Trim());
        //            SkillsRequired = skills;
        //        }
        //    }
        //}

        //public void RemoveSkill(string skill)
        //{
        //    if (!string.IsNullOrWhiteSpace(skill))
        //    {
        //        var skills = SkillsRequired;
        //        var skillToRemove = skills.FirstOrDefault(s => s.Equals(skill, StringComparison.OrdinalIgnoreCase));
        //        if (skillToRemove != null)
        //        {
        //            skills.Remove(skillToRemove);
        //            SkillsRequired = skills;
        //        }
        //    }
        //}

        //public void ClearSkills()
        //{
        //    SkillsRequired = new List<string>();
        //}
    }
}
