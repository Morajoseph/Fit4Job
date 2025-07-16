namespace Fit4Job.ViewModels.JobSeekerProfileViewModels
{
    public class JobSeekerProfileViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string? CvFileUrl { get; set; }
        public string? LinkedinUrl { get; set; }
        public string? GithubUrl { get; set; }
        public string? PortfolioUrl { get; set; }
        public int ExperienceYears { get; set; }
        public string? CurrentPosition { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public int UserCredit { get; set; }
        public string? Location { get; set; }

        public string Email { get; set; } = string.Empty;

        public static JobSeekerProfileViewModel FromEntity(JobSeekerProfile profile)
        {
            return new JobSeekerProfileViewModel
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FullName = $"{profile.FirstName} {profile.LastName}",
                CvFileUrl = profile.CvFileUrl,
                LinkedinUrl = profile.LinkedinUrl,
                GithubUrl = profile.GithubUrl,
                PortfolioUrl = profile.PortfolioUrl,
                ExperienceYears = profile.ExperienceYears,
                CurrentPosition = profile.CurrentPosition,
                ExpectedSalary = profile.ExpectedSalary,
                UserCredit = profile.UserCredit,
                Location = profile.Location,
                Email = profile.User?.Email ?? ""
            };
        }
    }
}

