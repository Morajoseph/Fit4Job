namespace Fit4Job.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAdminProfile(AdminRegistrationDTO registrationDTO, int userId)
        {
            var admin = registrationDTO.ToEntity(userId);
            await _unitOfWork.AdminProfileRepository.AddAsync(admin);
            await _unitOfWork.CompleteAsync();
        }

        public async Task CreateCompanyProfile(CompanyRegistrationDTO registrationDTO, int userId)
        {
            var company = registrationDTO.ToEntity(userId);
            await _unitOfWork.CompanyProfileRepository.AddAsync(company);
            await _unitOfWork.CompleteAsync();
        }

        public async Task CreateJobSeekerProfile(JobSeekerRegistrationDTO registrationDTO, int userId)
        {
            var jobSeeker = registrationDTO.ToEntity(userId);
            await _unitOfWork.JobSeekerProfileRepository.AddAsync(jobSeeker);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<ApiResponse<bool>> UpdateCoverPicture(IFormFile picture, ApplicationUser user)
        {
            var validationError = IsValidPicture(picture);
            if (validationError != null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, validationError);
            }

            var fileName = await SavePicture(picture);

            user.CoverPictureURL = $"/uploads/ProfilePictures/{fileName}";
            _unitOfWork.ApplicationUserRepository.Update(user);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Profile Picture Updated.");
        }

        public async Task<ApiResponse<bool>> UpdateProfilePicture(IFormFile picture, ApplicationUser user)
        {
            var validationError = IsValidPicture(picture);
            if (validationError != null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, validationError);
            }

            var fileName = await SavePicture(picture);


            user.ProfilePictureURL = $"/uploads/ProfilePictures/{fileName}";
            _unitOfWork.ApplicationUserRepository.Update(user);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Profile Picture Updated.");

        }

        /* ****************************************** Helper Functions ****************************************** */

        private string? IsValidPicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "Profile Picture is required.";

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                return "Invalid file type. Only JPG, PNG, GIF, and WebP files are allowed.";

            if (file.Length > 5 * 1024 * 1024) // 5MB limit
                return "File size must be less than 5MB.";

            return null;
        }

        private async Task<string> SavePicture(IFormFile picture)
        {
            var fileName = $"{Guid.NewGuid()}_{picture.FileName}";
            var uploadDir = Path.Combine("wwwroot", "uploads", "ProfilePictures");
            Directory.CreateDirectory(uploadDir);
            var filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }
            return fileName;
        }
    }
}
