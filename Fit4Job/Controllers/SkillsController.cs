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
            var seachedSkills = skills.Where(s => s.Name.Contains(keyword)).ToList();

            if (!skills.Any())
            {
                return ApiResponseHelper.Error<IEnumerable<SkillViewModel>>(ErrorCode.NotFound, "No skills found.");
            }


            var data = skills.Select(s => SkillViewModel.GetViewModel(s));
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