using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fit4Job.SystemDTOs.AccountDTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly RoleManager<IdentityRole<int>> roleManager;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config, RoleManager<IdentityRole<int>> roleManager)
        {

            this.userManager = userManager;
            this.config = config;
            this.roleManager = roleManager;
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

    }


    }

