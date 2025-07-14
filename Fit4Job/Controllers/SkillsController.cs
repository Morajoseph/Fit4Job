using Fit4Job.DTOs.SkillsDTOs;
using Fit4Job.ViewModels.Responses;
using Fit4Job.ViewModels.SkillsViewModels;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public SkillsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        /* ****************************************** Endpoints ****************************************** */



        //[HttpGet]
        //public async Task<ApiResponse<IEnumerable<SkillViewModel>>> GetSkills()
        //{

        //}

        //[HttpGet("{id}")]
        //public async Task<ApiResponse<SkillViewModel>> GetSkill(int id)
        //{

        //}

        //[HttpGet("search/{keyword}")]
        //public async Task<ApiResponse<IEnumerable<SkillViewModel>>> SearchSkills(string keyword)
        //{

        //}

        //[HttpPost]
        //public async Task<ApiResponse<SkillViewModel>> CreateSkill(CreateSkillDTO createSkillDTO)
        //{

        //}

        //[HttpPut("{id}")]
        //public async Task<ApiResponse<SkillViewModel>> UpdateSkill(int id, EditSkillDTO editSkillDTO)
        //{

        //}

        //[HttpDelete("{id}")]
        //public async Task<ApiResponse<bool>> DeleteSkill(int id)
        //{

        //}

        //[HttpPost("{id}/activate")]
        //public async Task<ApiResponse<bool>> ActivateSkill(int id)
        //{

        //}

        //[HttpPost("{id}/deactivate")]
        //public async Task<ApiResponse<bool>> DeactivateSkill(int id)
        //{

        //}

    }
}