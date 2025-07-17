using Fit4Job.DTOs.CompanyProfileDTOs;
using Fit4Job.DTOs.JobsDTOs;
using Fit4Job.Models;
using Fit4Job.ViewModels.CompanyExamQuestionViewModels;
using Fit4Job.ViewModels.CompanyProfileViewModels;
using Fit4Job.ViewModels.JobsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyProfilesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyProfilesController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        /* ****************************************** Endpoints ****************************************** */


        [HttpGet]
        public async Task<ApiResponse<IEnumerable<CompanyProfileViewModel>>> GetAllCompanyProfiles()
        {

            var companyProfiles = await _unitOfWork.CompanyProfileRepository.GetAllAsync();
            var data = companyProfiles.Select(cp => CompanyProfileViewModel.GetViewModel(cp));
            return ApiResponseHelper.Success(data);

        }



        [HttpGet("{id:int}")]
        public async Task<ApiResponse<CompanyProfileViewModel>> GetById(int id)
        {
            var companyProfile = await _unitOfWork.CompanyProfileRepository.GetByIdAsync(id);
            if (companyProfile == null)
            {
                return ApiResponseHelper.Error<CompanyProfileViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            return ApiResponseHelper.Success(new CompanyProfileViewModel(companyProfile));
        }



        [HttpPut("{id:int}")]
        public async Task<ApiResponse<CompanyProfileViewModel>> Update(int id, EditCompanyProfileDTO editCompanyProfileDTO)
        {
            if (editCompanyProfileDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyProfileViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var companyProfile = await _unitOfWork.CompanyProfileRepository.GetByIdAsync(id);
            if (companyProfile == null)
            {
                return ApiResponseHelper.Error<CompanyProfileViewModel>(ErrorCode.NotFound, "company not found");
            }

            editCompanyProfileDTO.UpdateEntity(companyProfile);
            _unitOfWork.CompanyProfileRepository.Update(companyProfile);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(CompanyProfileViewModel.GetViewModel(companyProfile), "Updated successfully");
        }


        [HttpDelete("{id:int}")]
        public async Task<ApiResponse<string>> SoftDelete(int id)
        {
            var companyProfile = await _unitOfWork.CompanyProfileRepository.GetByIdAsync(id);
            if (companyProfile == null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.NotFound, "company not found.");
            }
            if (companyProfile.DeletedAt != null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.BadRequest, "company is already deleted.");
            }
            companyProfile.DeletedAt = DateTime.UtcNow;
            companyProfile.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.CompanyProfileRepository.Update(companyProfile);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success("company is deleted successfully.");
        }
        [HttpGet("pending")]
      //  [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<IEnumerable<CompanyProfileViewModel>>> GetPendingCompanyProfiles()
        {
            var pendingCompanies = await _unitOfWork.CompanyProfileRepository.GetPendingCompaniesAsync();

            if (!pendingCompanies.Any())
            {
                return ApiResponseHelper.Error<IEnumerable<CompanyProfileViewModel>>(ErrorCode.NotFound, "No pending companies found");
            }

            var data = pendingCompanies.Select(CompanyProfileViewModel.GetViewModel);
            return ApiResponseHelper.Success(data, "Pending companies retrieved successfully");
        }




        [HttpGet("current")]
     // [Authorize(Roles = "Company")]
        public async Task<ApiResponse<CompanyProfileViewModel>> GetCurrentCompanyProfile()
        {
           
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return ApiResponseHelper.Error<CompanyProfileViewModel>( ErrorCode.Unauthorized, "Invalid user authentication");
            }


            var companyProfile = await _unitOfWork.CompanyProfileRepository.GetByUserIdAsync(userId);

            if (companyProfile == null)
            {
                return ApiResponseHelper.Error<CompanyProfileViewModel>(
                    ErrorCode.NotFound, "Company profile not found");
            }

           
            if (!companyProfile.IsActive)
            {
                return ApiResponseHelper.Error<CompanyProfileViewModel>( ErrorCode.NotFound, "Company profile is no longer active");
            }

            var viewModel = CompanyProfileViewModel.GetViewModel(companyProfile);
            return ApiResponseHelper.Success(viewModel, "Company profile retrieved successfully");
        }




    }
}
