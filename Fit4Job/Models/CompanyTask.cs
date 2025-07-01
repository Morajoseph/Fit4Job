namespace Fit4Job.Models
{
    [Table("company_tasks")]
    [Index(nameof(CompanyId), Name = "IX_CompanyTasks_CompanyId")]
    [Index(nameof(Deadline), Name = "IX_CompanyTasks_Deadline")]
    public class CompanyTask
    {
        [Key]
        [Display(Name = "Task ID")]
        public int Id { get; set; }


        [Required(ErrorMessage = "Company ID is required")]
        [Display(Name = "Company ID", Description = "Reference to the company that created this task")]
        [Range(1, int.MaxValue, ErrorMessage = "Company ID must be a positive number")]
        public int CompanyId { get; set; }


        [Required(ErrorMessage = "Task title is required")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Task title must be between 5 and 255 characters")]
        [Display(Name = "Title", Description = "Title or name of the task")]
        public string Title { get; set; } = string.Empty;


        [Required(ErrorMessage = "Task description is required")]
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Description", Description = "Detailed description of the task")]
        [StringLength(10000, MinimumLength = 20, ErrorMessage = "Task description must be between 20 and 10,000 characters")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;


        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Requirements", Description = "Specific requirements and qualifications needed")]
        [StringLength(5000, ErrorMessage = "Requirements cannot exceed 5,000 characters")]
        [DataType(DataType.MultilineText)]
        public string? Requirements { get; set; }


        [Column(TypeName = "nvarchar(5000)")]
        [Display(Name = "Deliverables", Description = "Expected deliverables and outcomes")]
        [StringLength(5000, ErrorMessage = "Deliverables cannot exceed 5,000 characters")]
        [DataType(DataType.MultilineText)]
        public string? Deliverables { get; set; }


        [Required(ErrorMessage = "Deadline is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; }


        [Display(Name = "Estimated Hours", Description = "Estimated time required to complete the task")]
        [Range(1, 10000, ErrorMessage = "Estimated hours must be between 1 and 10,000")]
        public int? EstimatedHours { get; set; }


        [Required]
        [Display(Name = "Is Active", Description = "Whether the task is currently active and accepting submissions")]
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
        public virtual CompanyProfile Company { get; set; } = null!;
        public virtual ICollection<CompanyTaskSubmission>? Submissions { get; set; }
    }
}
