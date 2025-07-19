namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyProfilesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProfileService _profileService;
        private readonly UserManager<ApplicationUser> _userManager;
        public CompanyProfilesController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IProfileService profileService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _profileService = profileService;
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

        [Authorize(Roles = "Company")]
        [HttpPut("profile/picture/{profileId:int}")]
        public async Task<ApiResponse<bool>> UpdateProfilePicture(int profileId, IFormFile companyProfilePicture)
        {
            var profile = await _unitOfWork.CompanyProfileRepository.GetByIdAsync(profileId);
            if (profile == null || profile.DeletedAt != null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "Profile not found.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.Unauthorized, "User not authenticated.");
            }

            if (profile.UserId != user.Id)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.Forbidden, "You are not authorized to update this profile picture.");
            }

            return await _profileService.UpdateProfilePicture(companyProfilePicture, user);
        }

        [Authorize(Roles = "Company")]
        [HttpPut("cover/picture/{profileId:int}")]
        public async Task<ApiResponse<bool>> UpdateCoverPicture(int profileId, IFormFile companyCoverPicture)
        {
            var profile = await _unitOfWork.CompanyProfileRepository.GetByIdAsync(profileId);
            if (profile == null || profile.DeletedAt != null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "Profile not found.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.Unauthorized, "User not authenticated.");
            }

            if (profile.UserId != user.Id)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.Forbidden, "You are not authorized to update this profile picture.");
            }

            return await _profileService.UpdateCoverPicture(companyCoverPicture, user);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
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

        [Authorize(Roles = "Company")]
        [HttpGet("current")]
        public async Task<ApiResponse<CompanyProfileViewModel>> GetCurrentCompanyProfile()
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return ApiResponseHelper.Error<CompanyProfileViewModel>(ErrorCode.Unauthorized, "Invalid user authentication");
            }


            var companyProfile = await _unitOfWork.CompanyProfileRepository.GetByUserIdAsync(userId);

            if (companyProfile == null)
            {
                return ApiResponseHelper.Error<CompanyProfileViewModel>(
                    ErrorCode.NotFound, "Company profile not found");
            }


            if (!companyProfile.IsActive)
            {
                return ApiResponseHelper.Error<CompanyProfileViewModel>(ErrorCode.NotFound, "Company profile is no longer active");
            }

            var viewModel = CompanyProfileViewModel.GetViewModel(companyProfile);
            return ApiResponseHelper.Success(viewModel, "Company profile retrieved successfully");
        }
    }
}