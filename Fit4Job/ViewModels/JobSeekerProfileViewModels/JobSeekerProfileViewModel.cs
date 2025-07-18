namespace Fit4Job.ViewModels.JobSeekerProfileViewModels
{
    public class JobSeekerProfileViewModel
    {
        [Display(Name = "Job Seeker Profile ID")]
        public int Id { get; set; }

        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "CV File URL")]
        public string? CvFileUrl { get; set; }

        [Display(Name = "LinkedIn URL")]
        public string? LinkedinUrl { get; set; }

        [Display(Name = "GitHub URL")]
        public string? GithubUrl { get; set; }

        [Display(Name = "Portfolio URL")]
        public string? PortfolioUrl { get; set; }

        [Display(Name = "Years of Experience")]
        public int ExperienceYears { get; set; } = 0;

        [Display(Name = "Current Position")]
        public string? CurrentPosition { get; set; }

        [Display(Name = "Expected Salary In USD")]
        public decimal? ExpectedSalary { get; set; }

        [Display(Name = "Location")]
        public string? Location { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Profile Picture URL")]
        public string? ProfilePictureURL { get; set; }

        [Display(Name = "Cover Picture URL")]
        public string? CoverPictureURL { get; set; }

        [Display(Name = "Bio")]
        public string? Bio { get; set; }

        [Display(Name = "Role")]
        public UserRole Role { get; set; }

        public static JobSeekerProfileViewModel GetViewModel(JobSeekerProfile profile)
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
                Location = profile.Location,
                Email = profile.User?.Email ?? "",
                ProfilePictureURL = profile.User?.ProfilePictureURL,
                CoverPictureURL = profile.User?.CoverPictureURL,
                Bio = profile.User?.Bio,
                Role = profile.User?.Role ?? UserRole.JobSeeker
            };
        }
    }
}

