namespace Fit4Job.DTOs.JobSeekerProfileDTOs
{
    public class JobSeekerProfileUpdateDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 20 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 20 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Url(ErrorMessage = "LinkedIn URL must be a valid URL.")]
        public string? LinkedinUrl { get; set; }

        [Url(ErrorMessage = "GitHub URL must be a valid URL.")]
        public string? GithubUrl { get; set; }

        [Url(ErrorMessage = "Portfolio URL must be a valid URL.")]
        public string? PortfolioUrl { get; set; }

        [Range(0, 50, ErrorMessage = "Experience years must be between 0 and 50.")]
        public int ExperienceYears { get; set; }

        [StringLength(50, ErrorMessage = "Current position must not exceed 50 characters.")]
        public string? CurrentPosition { get; set; }

        [Range(0, 1_000_000, ErrorMessage = "Expected salary must be a positive number.")]
        public decimal? ExpectedSalary { get; set; }

        [StringLength(100, ErrorMessage = "Location must not exceed 100 characters.")]
        public string? Location { get; set; }
    }
}
