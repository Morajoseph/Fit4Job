using Fit4Job.DTOs.JobSeekerProfileDTOs;
using Fit4Job.ViewModels.JobSeekerProfileViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace Fit4Job.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobSeekerProfilesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobSeekerProfilesController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: /api/JobSeekerProfile
        [HttpGet]
        public async Task<ApiResponse<PagedResultViewModel<JobSeekerProfileViewModel>>> GetAll(
            [FromQuery] string? location,
            [FromQuery] int? experienceYears,
            [FromQuery] string? currentPosition,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var pagedProfiles = await _unitOfWork.JobSeekerProfileRepository
                .GetFilteredProfilesAsync(location, experienceYears, currentPosition, page, pageSize);

            var result = new PagedResultViewModel<JobSeekerProfileViewModel>
            {
                Items = pagedProfiles.Items.Select(JobSeekerProfileViewModel.GetViewModel),
                TotalCount = pagedProfiles.TotalCount
            };

            return ApiResponseHelper.Success(result);
        }

        // GET: /api/JobSeekerProfile/{id}
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ApiResponse<JobSeekerProfileViewModel>> GetById(int id)
        {
            var profile = await _unitOfWork.JobSeekerProfileRepository.GetWithUserByIdAsync(id);
            if (profile == null || profile.DeletedAt != null)
            {
                return ApiResponseHelper.Error<JobSeekerProfileViewModel>(ErrorCode.NotFound, "Profile not found.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return ApiResponseHelper.Error<JobSeekerProfileViewModel>(ErrorCode.Unauthorized, "User not authenticated.");
            }
            return ApiResponseHelper.Success(JobSeekerProfileViewModel.GetViewModel(profile));
        }

        // GET: /api/JobSeekerProfile/current
        [HttpGet("current")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<ApiResponse<JobSeekerProfileViewModel>> GetCurrent()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return ApiResponseHelper.Error<JobSeekerProfileViewModel>(ErrorCode.Unauthorized, "User not authenticated.");
            }

            var profile = await _unitOfWork.JobSeekerProfileRepository.GetWithUserByUserIdAsync(user.Id);
            if (profile == null)
            {
                return ApiResponseHelper.Error<JobSeekerProfileViewModel>(ErrorCode.NotFound, "Profile not found.");
            }

            return ApiResponseHelper.Success(JobSeekerProfileViewModel.GetViewModel(profile));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,JobSeeker")]
        public async Task<ApiResponse<bool>> Update(int id, [FromBody] JobSeekerProfileUpdateDto UpdateDto)
        {
            var profile = await _unitOfWork.JobSeekerProfileRepository.GetByIdAsync(id);
            if (profile == null || profile.DeletedAt != null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "Profile not found.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.Unauthorized, "User not authenticated.");
            }
            else if (profile.UserId != user.Id)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.Forbidden, "Unauthorized to update this profile.");
            }


            if (UpdateDto.FirstName != null)
                profile.FirstName = UpdateDto.FirstName;

            if (UpdateDto.LastName != null)
                profile.LastName = UpdateDto.LastName;

            if (UpdateDto.LinkedinUrl != null)
                profile.LinkedinUrl = UpdateDto.LinkedinUrl;

            if (UpdateDto.GithubUrl != null)
                profile.GithubUrl = UpdateDto.GithubUrl;

            if (UpdateDto.PortfolioUrl != null)
                profile.PortfolioUrl = UpdateDto.PortfolioUrl;

            if (UpdateDto.ExperienceYears != 0)
                profile.ExperienceYears = UpdateDto.ExperienceYears;

            if (UpdateDto.CurrentPosition != null)
                profile.CurrentPosition = UpdateDto.CurrentPosition;

            if (UpdateDto.ExpectedSalary != null)
                profile.ExpectedSalary = UpdateDto.ExpectedSalary;

            if (UpdateDto.Location != null)
                profile.Location = UpdateDto.Location;

            profile.UpdatedAt = DateTime.UtcNow;



            _unitOfWork.JobSeekerProfileRepository.Update(profile);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Profile updated successfully.");
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpPut("profile/picture/{profileId:int}")]
        public async Task<ApiResponse<bool>> UpdateProfilePicture(int profileId, IFormFile profilePicture)
        {
            if (profilePicture == null || profilePicture.Length == 0)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Profile Picture is required.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(profilePicture.FileName)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Invalid file type. Only JPG, PNG, GIF, and WebP files are allowed.");
            }

            if (profilePicture.Length > 5 * 1024 * 1024) // 5MB limit
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "File size must be less than 5MB.");
            }


            var profile = await _unitOfWork.JobSeekerProfileRepository.GetByIdAsync(profileId);
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

            var fileName = $"{Guid.NewGuid()}_{profilePicture.FileName}";
            var uploadDir = Path.Combine("wwwroot", "uploads", "ProfilePictures");
            Directory.CreateDirectory(uploadDir);
            var filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            user.ProfilePictureURL = $"/uploads/ProfilePictures/{fileName}";
            _unitOfWork.ApplicationUserRepository.Update(user);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Profile Picture Updated.");
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpPut("cover/picture/{profileId:int}")]
        public async Task<ApiResponse<bool>> UpdateCoverPicture(int profileId, IFormFile coverPicture)
        {
            if (coverPicture == null || coverPicture.Length == 0)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Profile Picture is required.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(coverPicture.FileName)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Invalid file type. Only JPG, PNG, GIF, and WebP files are allowed.");
            }

            if (coverPicture.Length > 5 * 1024 * 1024) // 5MB limit
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "File size must be less than 5MB.");
            }


            var profile = await _unitOfWork.JobSeekerProfileRepository.GetByIdAsync(profileId);
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

            var fileName = $"{Guid.NewGuid()}_{coverPicture.FileName}";
            var uploadDir = Path.Combine("wwwroot", "uploads", "CoverPictures");
            Directory.CreateDirectory(uploadDir);
            var filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await coverPicture.CopyToAsync(stream);
            }

            user.CoverPictureURL = $"/uploads/CoverPictures/{fileName}";
            _unitOfWork.ApplicationUserRepository.Update(user);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Cover Picture Updated.");
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<bool>> SoftDelete(int id)
        {
            var profile = await _unitOfWork.JobSeekerProfileRepository.GetByIdAsync(id);
            if (profile == null || profile.DeletedAt != null)
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "Profile not found.");

            profile.DeletedAt = DateTime.UtcNow;
            _unitOfWork.JobSeekerProfileRepository.Update(profile);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Profile deleted successfully.");
        }

        [HttpPost("current/cv")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<ApiResponse<JobSeekerProfileViewModel>> UploadCV(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return ApiResponseHelper.Error<JobSeekerProfileViewModel>(ErrorCode.BadRequest, "File is required.");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var profile = await _unitOfWork.JobSeekerProfileRepository.GetWithUserByUserIdAsync(userId);

            if (profile == null)
                return ApiResponseHelper.Error<JobSeekerProfileViewModel>(ErrorCode.NotFound, "Profile not found.");

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var uploadDir = Path.Combine("wwwroot", "uploads", "cvs");
            Directory.CreateDirectory(uploadDir);
            var filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            profile.CvFileUrl = $"/uploads/cvs/{fileName}";
            _unitOfWork.JobSeekerProfileRepository.Update(profile);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(JobSeekerProfileViewModel.GetViewModel(profile), "CV uploaded.");
        }

        [HttpDelete("current/cv")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<ApiResponse<bool>> DeleteCV()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var profile = await _unitOfWork.JobSeekerProfileRepository.GetByUserIdAsync(userId);

            if (profile == null || string.IsNullOrEmpty(profile.CvFileUrl))
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "CV not found.");

            var filePath = Path.Combine("wwwroot", profile.CvFileUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            profile.CvFileUrl = null;
            _unitOfWork.JobSeekerProfileRepository.Update(profile);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "CV removed.");
        }
    }
}
