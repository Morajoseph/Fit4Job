using Fit4Job.DTOs.JobsDTOs;
using Fit4Job.DTOs.SkillsDTOs;
using Fit4Job.Migrations;
using Fit4Job.ViewModels.JobsViewModels;
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



        [HttpGet]
        public async Task<ApiResponse<IEnumerable<SkillViewModel>>> GetSkills()
        {
          var skills= await _unitOfWork.SkillRepository.GetAllAsync();
            var data= skills.Select(s=>SkillViewModel.GetViewModel(s));
            return ApiResponseHelper.Success(data);

        }




        [HttpGet("{id}")]
        public async Task<ApiResponse<SkillViewModel>> GetSkill(int id)
        {
            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(id);
            if (skill == null)
            {
                return ApiResponseHelper.Error<SkillViewModel>(ErrorCode.NotFound, "Skill is not found");
            }

            return ApiResponseHelper.Success(new SkillViewModel(skill));



        }






        [HttpGet("search/{keyword}")]
        public async Task<ApiResponse<IEnumerable<SkillViewModel>>> SearchSkills(string keyword)
        {
            var skills = await _unitOfWork.SkillRepository.GetAllAsync();

            if (!skills.Any())
            {
                return ApiResponseHelper.Error<IEnumerable<SkillViewModel>>(ErrorCode.NotFound, "No skills found.");
            }

            keyword = keyword.Trim();

            var seachedSkills = skills
      .Where(s => s.Name != null && s.Name.ToLower().Contains(keyword.ToLower()))
      .ToList();

            var data = seachedSkills.Select(s => SkillViewModel.GetViewModel(s));
            return ApiResponseHelper.Success(data);
        }



        [HttpPost]
        public async Task<ApiResponse<SkillViewModel>> CreateSkill(CreateSkillDTO createSkillDTO)
        {
            if (createSkillDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<SkillViewModel>(ErrorCode.BadRequest, "Invalid data");
            }


            var skill = createSkillDTO.ToEntity();
            await _unitOfWork.SkillRepository.AddAsync(skill);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(SkillViewModel.GetViewModel(skill), "Created successfully");

        }







        [HttpPut("{id}")]
        public async Task<ApiResponse<SkillViewModel>> UpdateSkill(int id, EditSkillDTO editSkillDTO)
        {
            if (editSkillDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<SkillViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(id);
            if (skill == null)
            {
                return ApiResponseHelper.Error<SkillViewModel>(ErrorCode.NotFound, "skill not found");
            }

            editSkillDTO.UpdateEntity(skill);
            _unitOfWork.SkillRepository.Update(skill);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(SkillViewModel.GetViewModel(skill), "Updated successfully");

        }





        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> DeleteSkill(int id)
        {
            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(id);
            if (skill == null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.NotFound, "skill not found.");
            }
            if (skill.DeletedAt != null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.BadRequest, "skill is already deleted.");
            }
            skill.DeletedAt = DateTime.UtcNow;

            _unitOfWork.SkillRepository.Update(skill);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success("skill is deleted successfully.");

        }





        [HttpPatch("{id}/activate")]
        public async Task<ApiResponse<SkillViewModel>> ActivateSkill(int id)
        {
            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(id);
            if (skill == null)
            {
                return ApiResponseHelper.Error<SkillViewModel>(ErrorCode.NotFound, "skill not found or deleted.");
            }

            if (skill.DeletedAt == null)
            {
                return ApiResponseHelper.Error<SkillViewModel>(ErrorCode.BadRequest, "skill is already active.");
            }

            skill.DeletedAt = null;
            _unitOfWork.SkillRepository.Update(skill);
            await _unitOfWork.CompleteAsync();

            var skillVm = new SkillViewModel(skill);
            return ApiResponseHelper.Success(skillVm);

        }








        [HttpPatch("{id}/deactivate")]
        public async Task<ApiResponse<SkillViewModel>> DeactivateSkill(int id)
        {
            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(id);
            if (skill == null)
            {
                return ApiResponseHelper.Error<SkillViewModel>(ErrorCode.NotFound, "Skill not found.");
            }

            if (skill.DeletedAt != null)
            {
                return ApiResponseHelper.Error<SkillViewModel>(ErrorCode.BadRequest, "Skill is already deactivated.");
            }

            skill.DeletedAt = DateTime.UtcNow; 
            _unitOfWork.SkillRepository.Update(skill);
            await _unitOfWork.CompleteAsync();

            var skillVm = new SkillViewModel(skill);
            return ApiResponseHelper.Success(skillVm);
        }














    }
}