namespace Fit4Job.ViewModels.CompanyTasksViewModels
{
    public class CompanyTaskViewModel
    {
        [Display(Name = "Task ID")]
        public int Id { get; set; }

        [Display(Name = "Company ID", Description = "Reference to the company that created this task")]
        public int CompanyId { get; set; }

        [Display(Name = "Job ID")]
        public int JobId { get; set; }

        [Display(Name = "Title", Description = "Title or name of the task")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Description", Description = "Detailed description of the task")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Requirements", Description = "Specific requirements and qualifications needed")]
        public string? Requirements { get; set; }

        [Display(Name = "Deliverables", Description = "Expected deliverables and outcomes")]
        public string? Deliverables { get; set; }

        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; }

        [Display(Name = "Estimated Hours", Description = "Estimated time required to complete the task")]
        public int? EstimatedHours { get; set; }

        public CompanyTaskViewModel()
        {

        }

        public CompanyTaskViewModel(CompanyTask task)
        {
            Id = task.Id;
            CompanyId = task.CompanyId;
            JobId = task.JobId;
            Title = task.Title;
            Description = task.Description;
            Requirements = task.Requirements;
            Deliverables = task.Deliverables;
            Deadline = task.Deadline;
            EstimatedHours = task.EstimatedHours;
        }
        public static CompanyTaskViewModel GetViewModel(CompanyTask task)
        {
            return new CompanyTaskViewModel(task);
        }
    }
}
