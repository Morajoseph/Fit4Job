using Fit4Job.ViewModels.Responses;
using Fit4Job.ViewModels.SkillsViewModels;

namespace Fit4Job.Controllers
{
    [ApiController]
    [Route("api/users/{userId:int}/skills")]
    public class UserSkillsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserSkillsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        /* ****************************************** Endpoints ****************************************** */

        //[HttpGet]
        //public async Task<ApiResponse<IEnumerable<SkillViewModel>>> GetUserSkills(int userId)
        //{

        //}


        //[HttpPost("{skillId:int}")]
        //public async Task<ApiResponse<SkillViewModel>> AddSkillToUser(int userId, int skillId)
        //{

        //}


        //[HttpDelete("{skillId:int}")]
        //public async Task<ApiResponse<bool>> RemoveSkillFromUser(int userId, int skillId)
        //{

        //}


        //[HttpGet("active")]
        //public async Task<ApiResponse<IEnumerable<SkillViewModel>>> GetActiveUserSkills(int userId)
        //{

        //}


        //[HttpPost("{skillId}/restore")]
        //public async Task<ApiResponse<bool>> RestoreUserSkill(int userId, int skillId)
        //{

        //}
    }
}
