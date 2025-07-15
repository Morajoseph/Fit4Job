using Fit4Job.DTOs.SkillsDTOs;
using Fit4Job.ViewModels.SkillsViewModels;

namespace Fit4Job.Services.Interfaces
{
    public interface ISkillsService
    {
        Task<ApiResponse<IEnumerable<SkillViewModel>>> GetAllSkillsAsync();
        Task<ApiResponse<SkillViewModel>> GetSkillByIdAsync(int id);
        Task<ApiResponse<IEnumerable<SkillViewModel>>> SearchSkillsAsync(string keyword);
        Task<ApiResponse<SkillViewModel>> CreateSkillAsync(CreateSkillDTO createSkillDTO);
        Task<ApiResponse<SkillViewModel>> UpdateSkillAsync(int id, EditSkillDTO editSkillDTO);
        Task<ApiResponse<string>> DeleteSkillAsync(int id);
        Task<ApiResponse<SkillViewModel>> ActivateSkillAsync(int id);
        Task<ApiResponse<SkillViewModel>> DeactivateSkillAsync(int id);
    }
}
