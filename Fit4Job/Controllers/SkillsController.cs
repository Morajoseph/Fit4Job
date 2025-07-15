using Fit4Job.DTOs.SkillsDTOs;
using Fit4Job.Services.Interfaces;
using Fit4Job.ViewModels.SkillsViewModels;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */
        private readonly ISkillsService _skillsService;

        public SkillsController(ISkillsService skillsService)
        {
            _skillsService = skillsService;
        }

        /* ****************************************** Endpoints ****************************************** */

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<SkillViewModel>>> GetSkills()
        {
            return await _skillsService.GetAllSkillsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<SkillViewModel>> GetSkill(int id)
        {
            return await _skillsService.GetSkillByIdAsync(id);
        }

        [HttpGet("search/{keyword}")]
        public async Task<ApiResponse<IEnumerable<SkillViewModel>>> SearchSkills(string keyword)
        {
            return await _skillsService.SearchSkillsAsync(keyword);
        }

        [HttpPost]
        public async Task<ApiResponse<SkillViewModel>> CreateSkill(CreateSkillDTO createSkillDTO)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.Error<SkillViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            return await _skillsService.CreateSkillAsync(createSkillDTO);
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<SkillViewModel>> UpdateSkill(int id, EditSkillDTO editSkillDTO)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.Error<SkillViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            return await _skillsService.UpdateSkillAsync(id, editSkillDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> DeleteSkill(int id)
        {
            return await _skillsService.DeleteSkillAsync(id);
        }

        [HttpPatch("{id}/activate")]
        public async Task<ApiResponse<SkillViewModel>> ActivateSkill(int id)
        {
            return await _skillsService.ActivateSkillAsync(id);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ApiResponse<SkillViewModel>> DeactivateSkill(int id)
        {
            return await _skillsService.DeactivateSkillAsync(id);
        }
    }
}