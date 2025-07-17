namespace Fit4Job.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<LoginViewModel>> Login(LoginDTO loginDTO);
        Task<ApiResponse<bool>> ResendVerificationCode(string email);
        Task<ApiResponse<bool>> EmailVerification(VerificationDTO verificationDTO);
        Task<ApiResponse<bool>> AdminRegistration(AdminRegistrationDTO registrationDTO);
        Task<ApiResponse<bool>> CompanyRegistration(CompanyRegistrationDTO registrationDTO);
        Task<ApiResponse<bool>> JobSeekerRegistration([FromBody] JobSeekerRegistrationDTO registrationDTO);
    }
}
