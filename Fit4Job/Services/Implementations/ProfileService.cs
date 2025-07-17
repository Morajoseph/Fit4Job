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
    }
}
