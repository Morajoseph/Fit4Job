using Fit4Job.DTOs.SkillsDTOs;
using Fit4Job.ViewModels.SkillsViewModels;

namespace Fit4Job.Services.Implementations
{
    public class SkillsService : ISkillsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<SkillViewModel>>> GetAllSkillsAsync()
        {
            var skills = await _unitOfWork.SkillRepository.GetAllAsync();
            var data = skills.Select(s => SkillViewModel.GetViewModel(s));
            return ApiResponseHelper.Success(data);
        }

        public async Task<ApiResponse<SkillViewModel>> GetSkillByIdAsync(int id)
        {
            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(id);
            if (skill == null)
            {
                return ApiResponseHelper.Error<SkillViewModel>(ErrorCode.NotFound, "Skill is not found");
            }

            return ApiResponseHelper.Success(new SkillViewModel(skill));
        }

        public async Task<ApiResponse<IEnumerable<SkillViewModel>>> SearchSkillsAsync(string keyword)
        {
            var skills = await _unitOfWork.SkillRepository.GetAllAsync();

            if (!skills.Any())
            {
                return ApiResponseHelper.Error<IEnumerable<SkillViewModel>>(ErrorCode.NotFound, "No skills found.");
            }

            keyword = keyword.Trim();

            var searchedSkills = skills
                .Where(s => s.Name != null && s.Name.ToLower().Contains(keyword.ToLower()))
                .ToList();

            var data = searchedSkills.Select(s => SkillViewModel.GetViewModel(s));
            return ApiResponseHelper.Success(data);
        }

        public async Task<ApiResponse<SkillViewModel>> CreateSkillAsync(CreateSkillDTO createSkillDTO)
        {
            if (createSkillDTO == null)
            {
                return ApiResponseHelper.Error<SkillViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var skill = createSkillDTO.ToEntity();
            await _unitOfWork.SkillRepository.AddAsync(skill);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(SkillViewModel.GetViewModel(skill), "Created successfully");
        }

        public async Task<ApiResponse<SkillViewModel>> UpdateSkillAsync(int id, EditSkillDTO editSkillDTO)
        {
            if (editSkillDTO == null)
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

        public async Task<ApiResponse<string>> DeleteSkillAsync(int id)
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

        public async Task<ApiResponse<SkillViewModel>> ActivateSkillAsync(int id)
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

        public async Task<ApiResponse<SkillViewModel>> DeactivateSkillAsync(int id)
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

