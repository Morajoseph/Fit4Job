using Fit4Job.Migrations;
using Fit4Job.ViewModels.JobsViewModels;
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

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<SkillViewModel>>> GetUserSkills(int userId)
        {
            var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponseHelper.Error<IEnumerable<SkillViewModel>>(ErrorCode.NotFound, "User not found.");
            }

            var skills = await _unitOfWork.UserSkillRepository.GetSkillsByUserIdAsync(userId);
            if (!skills.Any())
            {
                return ApiResponseHelper.Error<IEnumerable<SkillViewModel>>(ErrorCode.NotFound, "No skills found for this user.");
            }

            var data = skills.Select(t =>
            {
                var vm = SkillViewModel.GetViewModel(t.Skill);
                vm.IsActive = t.DeletedAt == null;
                return vm;
            });
            return ApiResponseHelper.Success(data);

        }



        [HttpPost("{skillId:int}")]
        public async Task<ApiResponse<SkillViewModel>> AddSkillToUser(int userId, int skillId)
        {
            var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponseHelper.Error<SkillViewModel>(
                    ErrorCode.NotFound, "User not found.");
            }

            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(skillId);
            if (skill == null)
            {
                return ApiResponseHelper.Error<SkillViewModel>(
                    ErrorCode.NotFound, "Skill not found.");
            }


            var existing = await _unitOfWork.UserSkillRepository
                .GetActiveUserSkillAsync(userId, skillId);
            if (existing != null)
            {
                return ApiResponseHelper.Error<SkillViewModel>(
                    ErrorCode.Conflict, "Skill is already added to the user.");
            }


            var userSkill = new UserSkill
            {
                UserId = userId,
                SkillId = skillId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.UserSkillRepository.AddAsync(userSkill);
            await _unitOfWork.CompleteAsync();


            var result = SkillViewModel.GetViewModel(skill);
            return ApiResponseHelper.Success(result, "Skill added to user successfully.");
        }


        [HttpDelete("{skillId:int}")]
        public async Task<ApiResponse<bool>> RemoveSkillFromUser(int userId, int skillId)
        {

            var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponseHelper.Error<bool>(
                    ErrorCode.NotFound, "User not found.");
            }


            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(skillId);
            if (skill == null)
            {
                return ApiResponseHelper.Error<bool>(
                    ErrorCode.NotFound, "Skill not found.");
            }

            var userSkill = await _unitOfWork.UserSkillRepository
           .GetActiveUserSkillAsync(userId, skillId);

            if (userSkill == null)
            {
                return ApiResponseHelper.Error<bool>(
                    ErrorCode.NotFound, "Skill not found for this user.");
            }

            userSkill.DeletedAt = DateTime.UtcNow;
            _unitOfWork.UserSkillRepository.Update(userSkill);
            await _unitOfWork.CompleteAsync();


            return ApiResponseHelper.Success(true, "Skill removed from user successfully.");

        }


        [HttpGet("active")]
        public async Task<ApiResponse<IEnumerable<SkillViewModel>>> GetActiveUserSkills(int userId)
        {
            var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponseHelper.Error<IEnumerable<SkillViewModel>>(ErrorCode.NotFound, "User not found.");
            }

            var activeSkills = await _unitOfWork.UserSkillRepository.GetActiveSkillsByUserIdAsync(userId);

            if (!activeSkills.Any())
            {
                return ApiResponseHelper.Error<IEnumerable<SkillViewModel>>(ErrorCode.NotFound, "No active skills found for this user.");
            }

            var data = activeSkills.Select(SkillViewModel.GetViewModel);
            return ApiResponseHelper.Success(data);
        }


        [HttpPost("{skillId}/restore")]
        public async Task<ApiResponse<bool>> RestoreUserSkill(int userId, int skillId)
        {
            var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "User not found.");
            }


            var deletedUserSkill = await _unitOfWork.UserSkillRepository.GetDeletedUserSkillAsync(userId, skillId);
            if (deletedUserSkill == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "No deleted skill found for this user.");
            }


            deletedUserSkill.DeletedAt = null;
            _unitOfWork.UserSkillRepository.Update(deletedUserSkill);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Skill restored for user successfully.");
        }
    }
}
