namespace Fit4Job.DTOs.JobsDTOs
{
    public class CreateJobDTO
    {
        [Required(ErrorMessage = "Company ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Company ID must be a positive number")]
        [Display(Name = "Company ID", Description = "Reference to the company that created this task")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Job title is required")]
        [Display(Name = "Title", Description = "Title or name of the task")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "Job title must be between 3 and 256 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job type is required")]
        [Display(Name = "Job Type", Description = "Type of job (Full-time , Part-time, etc.)")]
        [EnumDataType(typeof(JobType), ErrorMessage = "Invalid Job type")]
        public JobType JobType { get; set; }

        [Required(ErrorMessage = "Work Location type is required")]
        [Display(Name = "Work Location Type", Description = "Type of Work Location (Onsite , Remote, etc.)")]
        [EnumDataType(typeof(WorkLocationType), ErrorMessage = "Invalid Work Location type")]
        public WorkLocationType WorkLocationType { get; set; }

        [Display(Name = "Education Level", Description = "Education Level (High School , Bachelors Degree, etc.)")]
        [EnumDataType(typeof(EducationLevel), ErrorMessage = "Invalid Education Level")]
        public EducationLevel? EducationLevel { get; set; }

        [Required(ErrorMessage = "Job Summary is required")]
        [Display(Name = "Job Summary", Description = "Summary of the Job")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Job Summary must be between 10 and 2,000 characters")]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job Requirements is required")]
        [Display(Name = "Job Requirements", Description = "Requirements of the Job")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Job Requirements must be between 10 and 2,000 characters")]
        [DataType(DataType.MultilineText)]
        public string Requirements { get; set; } = string.Empty;

        [Display(Name = "Salary Range (Optional)")]
        [StringLength(50, ErrorMessage = "Salary range must not exceed 50 characters")]
        //[RegularExpression(@"^[\d,\$\s\-\.]+$", ErrorMessage = "Please enter a valid salary range (e.g., $80,000 - $120,000)")]
        public string? SalaryRange { get; set; }

        [Display(Name = "Years of Experience (Optional)")]
        [StringLength(30, ErrorMessage = "Experience range must not exceed 30 characters")]
        //[RegularExpression(@"^[\d\s\-\+years]+$", ErrorMessage = "Please enter a valid experience range (e.g., 3-5 years)")]
        public string? YearsOfExperience { get; set; }

        [Required]
        [Display(Name = "Is Active", Description = "Whether the Job is currently active")]
        public bool IsActive { get; set; } = true;


        public Job ToEntity()
        {
            return new Job()
            {
                CompanyId = this.CompanyId,
                Title = this.Title,
                JobType = this.JobType,
                WorkLocationType = this.WorkLocationType,
                EducationLevel = this.EducationLevel,
                Summary = this.Summary,
                Requirements = this.Requirements,
                SalaryRange = this.SalaryRange,
                YearsOfExperience = this.YearsOfExperience,
                IsActive = this.IsActive,
            };
        }
    }
}
