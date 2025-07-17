namespace Fit4Job.Services.Interfaces
{
    public interface IProfileService
    {
        Task CreateAdminProfile(AdminRegistrationDTO registrationDTO, int userId);
        Task CreateCompanyProfile(CompanyRegistrationDTO registrationDTO, int userId);
        Task CreateJobSeekerProfile(JobSeekerRegistrationDTO registrationDTO, int userId);
    }
}