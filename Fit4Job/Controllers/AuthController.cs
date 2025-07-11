namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdminProfileRepository adminProfileRepository;
        private readonly ICompanyProfileRepository companyProfileRepository;
        private readonly IJobSeekerProfileRepository jobSeekerProfileRepository;
        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config, IAdminProfileRepository adminProfileRepository, ICompanyProfileRepository companyProfileRepository, IJobSeekerProfileRepository jobSeekerProfileRepository)
        {
            this.config = config;
            this.userManager = userManager;
            this.adminProfileRepository = adminProfileRepository;
            this.companyProfileRepository = companyProfileRepository;
            this.jobSeekerProfileRepository = jobSeekerProfileRepository;
        }

        /* ****************************************** Endpoints ****************************************** */

        [HttpPost("Login")]
        public async Task<ApiResponse<LoginViewModel>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.Error<LoginViewModel>(ErrorCode.InvalidCredentials, "Invalid email/Username or password");
            }

            var user = await FindUserByEmailOrUsernameAsync(loginDTO.EmailOrUsername);
            if (user == null)
            {
                return ApiResponseHelper.Error<LoginViewModel>(ErrorCode.InvalidCredentials, "Invalid Credentials.");
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!isPasswordValid)
            {
                return ApiResponseHelper.Error<LoginViewModel>(ErrorCode.InvalidCredentials, "Invalid email/Username or password");
            }

            // Generate JWT token
            var token = await GenerateJwtTokenAsync(user, loginDTO.RememberMe);
            return ApiResponseHelper.Success(LoginViewModel.GetViewModel(token, user));
        }

        [HttpPost("Registration/Admin")]
        public async Task<ApiResponse<bool>> AdminRegistration(AdminRegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
                return ApiResponseHelper.Error<bool>(ErrorCode.ValidationError, "Validation Error");

            var isAlreadyExists = await IsAlreadyExists(registrationDTO);
            if (isAlreadyExists != ErrorCode.None)
                return ApiResponseHelper.Error<bool>(isAlreadyExists, "Email Or Username is already exists");

            ApplicationUser newUser = new ApplicationUser()
            {
                Role = UserRole.Admin,
                Email = registrationDTO.Email,
                UserName = registrationDTO.UserName
            };

            var result = await userManager.CreateAsync(newUser, registrationDTO.Password);
            if (!result.Succeeded)
            {
                StringBuilder errorsDescription = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errorsDescription.AppendLine(error.Description);
                }
                return ApiResponseHelper.Error<bool>(ErrorCode.ValidationError, errorsDescription.ToString());
            }

            await userManager.AddToRoleAsync(newUser, "Admin");
            int userId = newUser.Id;
            await CreateAdminProfile(registrationDTO, userId);

            return ApiResponseHelper.Success<bool>(true);
        }

        [HttpPost("Registration/Company")]
        public async Task<ApiResponse<bool>> CompanyRegistration(CompanyRegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
                return ApiResponseHelper.Error<bool>(ErrorCode.ValidationError, "Validation Error");

            var isAlreadyExists = await IsAlreadyExists(registrationDTO);
            if (isAlreadyExists != ErrorCode.None)
                return ApiResponseHelper.Error<bool>(isAlreadyExists, "Email Or Username is already exists");

            ApplicationUser newUser = new ApplicationUser()
            {
                Role = UserRole.Company,
                Email = registrationDTO.Email,
                UserName = registrationDTO.UserName
            };


            var result = await userManager.CreateAsync(newUser, registrationDTO.Password);

            if (!result.Succeeded)
            {
                StringBuilder errorsDescription = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errorsDescription.AppendLine(error.Description);
                }
                return ApiResponseHelper.Error<bool>(ErrorCode.ValidationError, errorsDescription.ToString());
            }

            await userManager.AddToRoleAsync(newUser, "Company");
            int userId = newUser.Id;
            await CreateCompanyProfile(registrationDTO, userId);

            return ApiResponseHelper.Success<bool>(true);
        }

        [HttpPost("Registration/JobSeeker")]
        public async Task<ApiResponse<bool>> JobSeekerRegistration(JobSeekerRegistrationDTO registrationDTO)
        {

            if (!ModelState.IsValid)
                return ApiResponseHelper.Error<bool>(ErrorCode.ValidationError, "Validation Error");

            var isAlreadyExists = await IsAlreadyExists(registrationDTO);
            if (isAlreadyExists != ErrorCode.None)
                return ApiResponseHelper.Error<bool>(isAlreadyExists, "Email Or Username is already exists");

            ApplicationUser newUser = new ApplicationUser()
            {
                Role = UserRole.JobSeeker,
                Email = registrationDTO.Email,
                UserName = registrationDTO.UserName
            };

            var result = await userManager.CreateAsync(newUser, registrationDTO.Password);
            if (!result.Succeeded)
            {
                StringBuilder errorsDescription = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errorsDescription.AppendLine(error.Description);
                }
                return ApiResponseHelper.Error<bool>(ErrorCode.ValidationError, errorsDescription.ToString());
            }

            await userManager.AddToRoleAsync(newUser, "JobSeeker");
            int userId = newUser.Id;
            await CreateJobSeekerProfile(registrationDTO, userId);

            return ApiResponseHelper.Success<bool>(true);
        }

        /* ****************************************** Helper Methods ****************************************** */
        private async Task<ErrorCode> IsAlreadyExists(BaseRegistrationDTO registrationDTO)
        {
            var existingUser = await userManager.FindByEmailAsync(registrationDTO.Email);
            if (existingUser != null)
            {
                return ErrorCode.EmailAlreadyExists;
            }

            existingUser = await userManager.FindByNameAsync(registrationDTO.Email);
            if (existingUser != null)
            {
                return ErrorCode.UsernameAlreadyExists;
            }

            return ErrorCode.None;
        }
        private async Task CreateAdminProfile(AdminRegistrationDTO registrationDTO, int userId)
        {
            AdminProfile admin = new AdminProfile()
            {
                UserId = userId,
                FirstName = registrationDTO.FirstName,
                LastName = registrationDTO.LastName,
            };
            await adminProfileRepository.AddAsync(admin);
            await adminProfileRepository.SaveChangesAsync();
        }
        private async Task CreateCompanyProfile(CompanyRegistrationDTO registrationDTO, int userId)
        {
            CompanyProfile company = new CompanyProfile()
            {
                UserId = userId,
                CompanyName = registrationDTO.CompanyName,
            };
            await companyProfileRepository.AddAsync(company);
            await companyProfileRepository.SaveChangesAsync();
        }
        private async Task CreateJobSeekerProfile(JobSeekerRegistrationDTO registrationDTO, int userId)
        {
            JobSeekerProfile jobSeeker = new JobSeekerProfile()
            {
                UserId = userId,
                FirstName = registrationDTO.FirstName,
                LastName = registrationDTO.LastName,
            };
            await jobSeekerProfileRepository.AddAsync(jobSeeker);
            await jobSeekerProfileRepository.SaveChangesAsync();
        }
        private async Task<(string Token, DateTime Expiration)> GenerateJwtTokenAsync(ApplicationUser user, bool rememberMe)
        {
            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64)
            };

            // Add role claims
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtKey = config["JWT:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT Key is not configured");
            }

            var authSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtKey));
            var expires = rememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddHours(12);

            var token = new JwtSecurityToken(
                issuer: config["JWT:Issuer"],
                //audience: config["JWT:Audience"], // Add audience
                expires: expires,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
        private async Task<ApplicationUser?> FindUserByEmailOrUsernameAsync(string emailOrUsername)
        {
            // Try to find by email first
            var user = await userManager.FindByEmailAsync(emailOrUsername);

            // If not found by email, try by username
            if (user == null)
            {
                user = await userManager.FindByNameAsync(emailOrUsername);
            }

            return user;
        }
    }
}