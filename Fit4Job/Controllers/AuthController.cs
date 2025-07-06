using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Fit4Job.DTOs.AuthorizationDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration config;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;
        private readonly IAdminProfileRepository adminProfileRepository;
        private readonly ICompanyProfileRepository companyProfileRepository;
        private readonly IJobSeekerProfileRepository jobSeekerProfileRepository;
        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config, 
            RoleManager<IdentityRole<int>> roleManager, AdminProfileRepository adminProfileRepository ,
            ICompanyProfileRepository companyProfileRepository , IJobSeekerProfileRepository jobSeekerProfileRepository)
        {

            this.config = config;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.adminProfileRepository = adminProfileRepository;
            this.companyProfileRepository = companyProfileRepository;
            this.jobSeekerProfileRepository = jobSeekerProfileRepository;
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByEmailAsync(loginDTO.Email); // Fixed variable name
            if (user == null)
                return Unauthorized("Invalid email");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginDTO.Password); // Fixed variable name
            if (!isPasswordValid)
                return Unauthorized("Invalid password");

            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: config["JWT:Iss"],
                expires: loginDTO.RememberMe ? DateTime.Now.AddDays(7) : DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                username = user.UserName,
                roles = userRoles
            });
        }


        [HttpPost("Registration/Admin")]
        public async Task<IActionResult> AdminRegistration(AdminRegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? isAlreadyExists = await IsAlreadyExists(registrationDTO);
            if (isAlreadyExists != null)
            {
                return BadRequest(isAlreadyExists);
            }

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
                return BadRequest(errorsDescription.ToString());
            }

            await userManager.AddToRoleAsync(newUser, "Admin");

            int userId = newUser.Id;

            await CreateAdminProfile(registrationDTO, userId);

            return Ok("Account Created Successfully");
        }


        [HttpPost("Registration/Company")]
        public async Task<IActionResult> CompanyRegistration(CompanyRegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? isAlreadyExists = await IsAlreadyExists(registrationDTO);
            if (isAlreadyExists != null)
            {
                return BadRequest(isAlreadyExists);
            }

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
                return BadRequest(errorsDescription.ToString());
            }

            await userManager.AddToRoleAsync(newUser, "Company");

            int userId = newUser.Id;

            await CreateCompanyProfile(registrationDTO, userId);


            return Ok("Account Created Successfully");
        }


        [HttpPost("Registration/JobSeeker")]
        public async Task<IActionResult> JobSeekerRegistration(JobSeekerRegistrationDTO registrationDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? isAlreadyExists = await IsAlreadyExists(registrationDTO);
            if (isAlreadyExists != null)
            {
                return BadRequest(isAlreadyExists);
            }

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
                return BadRequest(errorsDescription.ToString());
            }

            await userManager.AddToRoleAsync(newUser, "JobSeeker");

            int userId = newUser.Id;

            await CreateJobSeekerProfile(registrationDTO, userId);

            return Ok("Account Created Successfully");
        }

        /* ****************************************** Helper Methods ****************************************** */
        private async Task<string?> IsAlreadyExists(BaseRegistrationDTO registrationDTO)
        {
            var existingUser = await userManager.FindByEmailAsync(registrationDTO.Email);
            if (existingUser != null)
            {
                return "Email is already registered";
            }

            existingUser = await userManager.FindByNameAsync(registrationDTO.Email);
            if (existingUser != null)
            {
                return "Username is already used";
            }

            return null;
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
                CompanySize = registrationDTO.CompanySize
            };
            await companyProfileRepository.AddAsync(company);
            await companyProfileRepository.SaveChangesAsync();
        }
        private async Task CreateJobSeekerProfile(JobSeekerRegistrationDTO registrationDTO , int userId)
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
    }
}