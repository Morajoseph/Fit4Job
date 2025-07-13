namespace Fit4Job.ViewModels.JobsViewModels
{
    public class JobViewModel
    {
        [Display(Name = "Job ID")]
        public int Id { get; set; }

        [Display(Name = "Company ID", Description = "Reference to the company that created this task")]
        public int CompanyId { get; set; }

        [Display(Name = "Title", Description = "Title or name of the task")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Job Type", Description = "Type of job (Full-time , Part-time, etc.)")]
        public JobType JobType { get; set; }

        [Display(Name = "Work Location Type", Description = "Type of Work Location (Onsite , Remote, etc.)")]
        public WorkLocationType WorkLocationType { get; set; }

        [Display(Name = "Education Level", Description = "Education Level (High School , Bachelors Degree, etc.)")]
        public EducationLevel? EducationLevel { get; set; }

        [Display(Name = "Job Summary", Description = "Summary of the Job")]
        public string Summary { get; set; } = string.Empty;

        [Display(Name = "Job Requirements", Description = "Requirements of the Job")]
        public string Requirements { get; set; } = string.Empty;

        [Display(Name = "Salary Range (Optional)")]
        public string? SalaryRange { get; set; }

        [Display(Name = "Years of Experience (Optional)")]
        public string? YearsOfExperience { get; set; }

        public JobViewModel()
        {

        }

        public JobViewModel(Job job)
        {
            this.CompanyId = job.CompanyId ;
            this .Title = job.Title ;
            this.JobType = job.JobType;
            this.WorkLocationType = job.WorkLocationType;
            this.EducationLevel = job.EducationLevel;
            this.Summary = job.Summary;
            this.Requirements = job.Requirements;
            this.SalaryRange = job.SalaryRange;
            this.YearsOfExperience = job.YearsOfExperience;
        }

        public static JobViewModel GetViewModel(Job job)
        {
            return new JobViewModel(job);
        }
    }
}
