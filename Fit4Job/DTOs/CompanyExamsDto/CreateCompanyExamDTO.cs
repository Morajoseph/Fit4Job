namespace Fit4Job.DTOs.CompanyExamsDto
{
    public class CreateCompanyExamDto
    {
        [Display(Name = "Company ID")]
        [Required(ErrorMessage = "Company ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Company ID must be a positive value")]
        public int CompanyId { get; set; }

        [Display(Name = "Job ID")]
        [Required(ErrorMessage = "Job ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Job ID must be a positive value")]
        public int JobId { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title is required")]
        [StringLength(256, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 256 characters")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Display(Name = "Instructions")]
        [StringLength(2000, ErrorMessage = "Instructions cannot exceed 2000 characters")]
        public string? Instructions { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Display(Name = "Duration (Minutes)")]
        [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes")]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "Passing score is required")]
        [Display(Name = "Passing Score")]
        [Range(0, 100, ErrorMessage = "Passing score must be a positive value")]
        public decimal PassingScore { get; set; } = 0.00m;

        [Display(Name = "Show Results Immediately")]
        public bool ShowResultsImmediately { get; set; } = false;

        public CompanyExam ToEntity()
        {
            return new CompanyExam
            {
                CompanyId = this.CompanyId,
                JobId = this.JobId,
                Title = this.Title,
                Description = this.Description,
                Instructions = this.Instructions,
                DurationMinutes = this.DurationMinutes,
                TotalScore = 0,
                PassingScore = this.PassingScore,
                ShowResultsImmediately = this.ShowResultsImmediately,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
        }
    }
}