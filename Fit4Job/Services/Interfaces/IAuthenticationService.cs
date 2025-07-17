namespace Fit4Job.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<bool>> AdminRegistration(AdminRegistrationDTO registrationDTO);
        Task<ApiResponse<bool>> CompanyRegistration(CompanyRegistrationDTO registrationDTO);
        Task<ApiResponse<bool>> JobSeekerRegistration(JobSeekerRegistrationDTO registrationDTO);
        Task<ApiResponse<bool>> EmailVerification(VerificationDTO verificationDTO);
        Task<ApiResponse<LoginViewModel>> Login(LoginDTO loginDTO);
        Task<ApiResponse<bool>> ResendVerificationCode(string emailOrUsername);
    }
}
