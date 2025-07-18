using Fit4Job.DTOs.CompanyProfileDTOs;
using Fit4Job.ViewModels.CompanyProfileViewModels;

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

        [Authorize(Roles = "Company")]
        [HttpPut("profile/picture/{profileId:int}")]
        public async Task<ApiResponse<bool>> UpdateProfilePicture(int profileId, IFormFile companyProfilePicture)
        {
            if (companyProfilePicture == null || companyProfilePicture.Length == 0)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Company Profile Picture is required.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(companyProfilePicture.FileName)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Invalid file type. Only JPG, PNG, GIF, and WebP files are allowed.");
            }

            if (companyProfilePicture.Length > 5 * 1024 * 1024) // 5MB limit
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "File size must be less than 5MB.");
            }


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

            var fileName = $"{Guid.NewGuid()}_{companyProfilePicture.FileName}";
            var uploadDir = Path.Combine("wwwroot", "uploads", "ProfilePictures");
            Directory.CreateDirectory(uploadDir);
            var filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await companyProfilePicture.CopyToAsync(stream);
            }

            user.ProfilePictureURL = $"/uploads/ProfilePictures/{fileName}";
            _unitOfWork.ApplicationUserRepository.Update(user);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Profile Picture Updated.");
        }

        [Authorize(Roles = "Company")]
        [HttpPut("cover/picture/{profileId:int}")]
        public async Task<ApiResponse<bool>> UpdateCoverPicture(int profileId, IFormFile companyCoverPicture)
        {
            if (companyCoverPicture == null || companyCoverPicture.Length == 0)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Profile Picture is required.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(companyCoverPicture.FileName)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Invalid file type. Only JPG, PNG, GIF, and WebP files are allowed.");
            }

            if (companyCoverPicture.Length > 5 * 1024 * 1024) // 5MB limit
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "File size must be less than 5MB.");
            }


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

            var fileName = $"{Guid.NewGuid()}_{companyCoverPicture.FileName}";
            var uploadDir = Path.Combine("wwwroot", "uploads", "CoverPictures");
            Directory.CreateDirectory(uploadDir);
            var filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await companyCoverPicture.CopyToAsync(stream);
            }

            user.CoverPictureURL = $"/uploads/CoverPictures/{fileName}";
            _unitOfWork.ApplicationUserRepository.Update(user);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Cover Picture Updated.");
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