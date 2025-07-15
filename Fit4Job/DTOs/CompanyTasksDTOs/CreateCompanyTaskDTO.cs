namespace Fit4Job.DTOs.CompanyTasksDTOs
{
    public class CreateCompanyTaskDTO
    {
        [Required(ErrorMessage = "Company ID is required")]
        [Display(Name = "Company ID", Description = "Reference to the company that created this task")]
        [Range(1, int.MaxValue, ErrorMessage = "Company ID must be a positive number")]
        public int CompanyId { get; set; }

        [Display(Name = "Job ID")]
        [Required(ErrorMessage = "Job ID is required")]
        public int JobId { get; set; }

        [Required(ErrorMessage = "Task title is required")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Task title must be between 5 and 255 characters")]
        [Display(Name = "Title", Description = "Title or name of the task")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Task description is required")]
        [Display(Name = "Description", Description = "Detailed description of the task")]
        [StringLength(4000, MinimumLength = 20, ErrorMessage = "Task description must be between 20 and 4,000 characters")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Requirements", Description = "Specific requirements and qualifications needed")]
        [StringLength(4000, ErrorMessage = "Requirements cannot exceed 4,000 characters")]
        [DataType(DataType.MultilineText)]
        public string? Requirements { get; set; }

        [Display(Name = "Deliverables", Description = "Expected deliverables and outcomes")]
        [StringLength(4000, ErrorMessage = "Deliverables cannot exceed 4,000 characters")]
        [DataType(DataType.MultilineText)]
        public string? Deliverables { get; set; }

        [Required(ErrorMessage = "Deadline is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; }

        [Display(Name = "Estimated Hours", Description = "Estimated time required to complete the task")]
        [Range(1, 10000, ErrorMessage = "Estimated hours must be between 1 and 10,000")]
        public int? EstimatedHours { get; set; }

        public CompanyTask ToEntity()
        {
            return new CompanyTask()
            {
                CompanyId = this.CompanyId,
                JobId = this.JobId,
                Title = this.Title,
                Deliverables = this.Deliverables,
                Requirements = this.Requirements,
                Description = this.Description,
                Deadline = this.Deadline,
                EstimatedHours = this.EstimatedHours
            };
        }
    }
}
