namespace Fit4Job.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IProfileService _profileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(IUnitOfWork unitOfWork, IConfiguration configuration, IEmailService emailService, IProfileService profileService, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
            _profileService = profileService;
        }

        public async Task<ApiResponse<bool>> AdminRegistration(AdminRegistrationDTO registrationDTO)
        {
            var isAlreadyExists = await IsAlreadyExists(registrationDTO);
            if (isAlreadyExists != ErrorCode.None)
            {
                return ApiResponseHelper.Error<bool>(isAlreadyExists, isAlreadyExists.ToString());
            }

            ApplicationUser newUser = new ApplicationUser()
            {
                Role = UserRole.Admin,
                Email = registrationDTO.Email,
                UserName = registrationDTO.UserName
            };

            var result = await _userManager.CreateAsync(newUser, registrationDTO.Password);
            if (!result.Succeeded)
            {
                StringBuilder errorsDescription = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errorsDescription.AppendLine(error.Description);
                }
                return ApiResponseHelper.Error<bool>(ErrorCode.ValidationError, errorsDescription.ToString());
            }

            await _userManager.AddToRoleAsync(newUser, "Admin");
            int userId = newUser.Id;
            await _profileService.CreateAdminProfile(registrationDTO, userId);

            var code = await EmailConfirmation(newUser, registrationDTO.FirstName + ' ' + registrationDTO.LastName);

            return ApiResponseHelper.Success(true);
        }

        public async Task<ApiResponse<bool>> CompanyRegistration(CompanyRegistrationDTO registrationDTO)
        {
            var isAlreadyExists = await IsAlreadyExists(registrationDTO);
            if (isAlreadyExists != ErrorCode.None)
            {
                return ApiResponseHelper.Error<bool>(isAlreadyExists, isAlreadyExists.ToString());
            }

            ApplicationUser newUser = new ApplicationUser()
            {
                Role = UserRole.Company,
                Email = registrationDTO.Email,
                UserName = registrationDTO.UserName
            };


            var result = await _userManager.CreateAsync(newUser, registrationDTO.Password);
            if (!result.Succeeded)
            {
                StringBuilder errorsDescription = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errorsDescription.AppendLine(error.Description);
                }
                return ApiResponseHelper.Error<bool>(ErrorCode.ValidationError, errorsDescription.ToString());
            }

            await _userManager.AddToRoleAsync(newUser, "Company");
            int userId = newUser.Id;
            await _profileService.CreateCompanyProfile(registrationDTO, userId);

            var code = await EmailConfirmation(newUser, registrationDTO.CompanyName);

            return ApiResponseHelper.Success(true);
        }

        public async Task<ApiResponse<bool>> EmailVerification(VerificationDTO verificationDTO)
        {
            var user = await FindUserByEmailOrUsernameAsync(verificationDTO.EmailOrUsername);
            if (user is null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.Unauthorized, "User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, verificationDTO.VerificationCode);
            if (!result.Succeeded)
            {
                var details = string.Join(" | ", result.Errors.Select(e => e.Description));
                return ApiResponseHelper.Error<bool>(ErrorCode.InvalidCredentials, $"Email verification failed: {details}");
            }
            return ApiResponseHelper.Success(true, "Email verified successfully.");
        }

        public async Task<ApiResponse<bool>> JobSeekerRegistration(JobSeekerRegistrationDTO registrationDTO)
        {
            var isAlreadyExists = await IsAlreadyExists(registrationDTO);
            if (isAlreadyExists != ErrorCode.None)
            {
                return ApiResponseHelper.Error<bool>(isAlreadyExists, isAlreadyExists.ToString());
            }

            ApplicationUser newUser = new ApplicationUser()
            {
                Role = UserRole.JobSeeker,
                Email = registrationDTO.Email,
                UserName = registrationDTO.UserName
            };

            var result = await _userManager.CreateAsync(newUser, registrationDTO.Password);
            if (!result.Succeeded)
            {
                StringBuilder errorsDescription = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errorsDescription.AppendLine(error.Description);
                }
                return ApiResponseHelper.Error<bool>(ErrorCode.ValidationError, errorsDescription.ToString());
            }

            await _userManager.AddToRoleAsync(newUser, "JobSeeker");
            int userId = newUser.Id;
            await _profileService.CreateJobSeekerProfile(registrationDTO, userId);

            var code = await EmailConfirmation(newUser, registrationDTO.FirstName + ' ' + registrationDTO.LastName);

            return ApiResponseHelper.Success(true);
        }

        public async Task<ApiResponse<LoginViewModel>> Login(LoginDTO loginDTO)
        {
            var user = await FindUserByEmailOrUsernameAsync(loginDTO.EmailOrUsername);
            if (user == null)
            {
                return ApiResponseHelper.Error<LoginViewModel>(ErrorCode.InvalidCredentials, "Invalid Credentials.");
            }

            if (!user.EmailConfirmed)
            {
                return ApiResponseHelper.Error<LoginViewModel>(ErrorCode.EmailNotConfirmed, "You must confirm your e‑mail before logging in.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!isPasswordValid)
            {
                return ApiResponseHelper.Error<LoginViewModel>(ErrorCode.InvalidCredentials, "Invalid email/Username or password");
            }

            var token = await GenerateJwtTokenAsync(user, loginDTO.RememberMe); // Generate JWT token
            return ApiResponseHelper.Success(LoginViewModel.GetViewModel(token, user, await GetUserProfileId(user)));
        }

        public async Task<ApiResponse<bool>> ResendVerificationCode(string emailOrUsername)
        {
            var user = await FindUserByEmailOrUsernameAsync(emailOrUsername);
            if (user == null)
            {
                return ApiResponseHelper.Success(true, "Think again.");
            }

            var code = await EmailConfirmation(user, user.UserName ?? emailOrUsername);
            if (code == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.UnknownError, "Failed to generate verification code.");
            }

            return ApiResponseHelper.Success(true);
        }

        /* ****************************************** Helper Methods ****************************************** */

        private async Task<ApplicationUser?> FindUserByEmailOrUsernameAsync(string emailOrUsername)
        {
            var user = await _userManager.FindByEmailAsync(emailOrUsername); // Try to find by email first
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(emailOrUsername); // If not found by email, try by username
            }

            return user;
        }
        private async Task<(string Token, DateTime Expiration)> GenerateJwtTokenAsync(ApplicationUser user, bool rememberMe)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

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

            var jwtKey = _configuration["JWT:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT Key is not configured");
            }

            var authSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtKey));
            var expires = rememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddHours(12);

            var token = new JwtSecurityToken(
                //audience: config["JWT:Audience"], // Add audience
                issuer: _configuration["JWT:Issuer"],
                expires: expires,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
        private async Task<ErrorCode> IsAlreadyExists(BaseRegistrationDTO registrationDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(registrationDTO.Email);
            if (existingUser != null)
            {
                return ErrorCode.EmailAlreadyExists;
            }

            existingUser = await _userManager.FindByNameAsync(registrationDTO.Email);
            if (existingUser != null)
            {
                return ErrorCode.UsernameAlreadyExists;
            }

            return ErrorCode.None;
        }
        private async Task<string?> EmailConfirmation(ApplicationUser newUser, string name)
        {
            if (string.IsNullOrEmpty(newUser.Email))
            {
                return null;
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var emailSubject = "Confirm Your Email Address - Fit4Job";
            var emailBody = CreatePlainTextEmailConfirmationBody(newUser, code, name);
            await _emailService.SendEmail(newUser.Email, emailSubject, emailBody);
            return code;
        }
        private string CreatePlainTextEmailConfirmationBody(ApplicationUser user, string confirmationCode, string userName)
        {
            return
                $@"Dear {userName}," +
                "\n\nThank you for registering with Fit4Job!" +
                $"\n\nTo complete your account setup, please use the following verification code: {confirmationCode}" +
                "\n\nBest regards," +
                "\n\nThe Fit4Job Team";
        }
        private async Task<int> GetUserProfileId(ApplicationUser user)
        {
            int profileId = 0;
            if (user.Role == UserRole.Admin)
            {
                var adminProfile = await _unitOfWork.AdminProfileRepository.GetByUserIdAsync(user.Id);
                profileId = adminProfile?.Id ?? 0;
            }
            else if (user.Role == UserRole.Company)
            {
                var companyProfile = await _unitOfWork.CompanyProfileRepository.GetByUserIdAsync(user.Id);
                profileId = companyProfile?.Id ?? 0;
            }
            else if (user.Role == UserRole.JobSeeker)
            {
                var jobSeekerProfile = await _unitOfWork.JobSeekerProfileRepository.GetByUserIdAsync(user.Id);
                profileId = jobSeekerProfile?.Id ?? 0;
            }
            return profileId;
        }
    }
}