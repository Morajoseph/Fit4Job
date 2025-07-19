namespace Fit4Job.Services.Interfaces
{
    public interface IProfileService
    {
        Task CreateAdminProfile(AdminRegistrationDTO registrationDTO, int userId);
        Task CreateCompanyProfile(CompanyRegistrationDTO registrationDTO, int userId);
        Task CreateJobSeekerProfile(JobSeekerRegistrationDTO registrationDTO, int userId);
        Task<ApiResponse<bool>> UpdateCoverPicture(IFormFile coverPicture, ApplicationUser user);
        Task<ApiResponse<bool>> UpdateProfilePicture(IFormFile profilePicture, ApplicationUser user);
    }
}