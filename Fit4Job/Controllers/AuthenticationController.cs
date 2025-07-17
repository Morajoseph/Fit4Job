namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */

        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /* ****************************************** Endpoints ****************************************** */

        [HttpPost("Login")]
        public async Task<ApiResponse<LoginViewModel>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.Error<LoginViewModel>(ErrorCode.InvalidCredentials, "Invalid email/Username or password");
            }

            return await _authenticationService.Login(loginDTO);
        }

        [HttpPost("Verification")]
        public async Task<ApiResponse<bool>> EmailVerification(VerificationDTO verificationDTO)
        {
            if (string.IsNullOrWhiteSpace(verificationDTO.EmailOrUsername) || string.IsNullOrWhiteSpace(verificationDTO.VerificationCode))
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.InvalidCredentials, "Invalid payload.");
            }
            return await _authenticationService.EmailVerification(verificationDTO);
        }

        [HttpPost("Verification-Code/Resend")]
        public async Task<ApiResponse<bool>> ResendVerificationCode(string emailOrUsername)
        {
            if (string.IsNullOrWhiteSpace(emailOrUsername))
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.InvalidCredentials, "Email Or Username is required.");
            }

            return await _authenticationService.ResendVerificationCode(emailOrUsername);
        }

        [HttpPost("Registration/Admin")]
        public async Task<ApiResponse<bool>> AdminRegistration(AdminRegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.ValidationError, "Validation Error");
            }

            return await _authenticationService.AdminRegistration(registrationDTO);
        }

        [HttpPost("Registration/Company")]
        public async Task<ApiResponse<bool>> CompanyRegistration(CompanyRegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.ValidationError, "Validation Error");
            }
            return await _authenticationService.CompanyRegistration(registrationDTO);
        }

        [HttpPost("Registration/JobSeeker")]
        public async Task<ApiResponse<bool>> JobSeekerRegistration(JobSeekerRegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.ValidationError, "Validation Error");
            }

            return await _authenticationService.JobSeekerRegistration(registrationDTO);
        }
    }
}