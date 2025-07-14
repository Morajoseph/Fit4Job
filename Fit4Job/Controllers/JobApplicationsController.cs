using Fit4Job.DTOs.JobApplicationsDTOs;
using Fit4Job.ViewModels.JobApplicationsViewModels;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public JobApplicationsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        /* ****************************************** Endpoints ****************************************** */

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<JobApplicationViewModel>>> GetAll()
        {
            var jobs = await _unitOfWork.JobApplicationRepository.GetAllAsync();
            var data = jobs.Select(t => JobApplicationViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResponse<JobApplicationViewModel>> GetById(int id)
        {
            var job = await _unitOfWork.JobApplicationRepository.GetByIdAsync(id);
            if (job == null)
            {
                return ApiResponseHelper.Error<JobApplicationViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            return ApiResponseHelper.Success(new JobApplicationViewModel(job));
        }

        [HttpPost]
        public async Task<ApiResponse<JobApplicationViewModel>> Create(CreateJobApplicationDTO createJobApplicationDTO )
        {
            if (createJobApplicationDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<JobApplicationViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var jobApplication = createJobApplicationDTO.ToEntity();
            await _unitOfWork.JobApplicationRepository.AddAsync(jobApplication);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(JobApplicationViewModel.GetViewModel(jobApplication), "Created successfully");
        }

        [HttpPut("{id:int}")]
        public async Task<ApiResponse<JobApplicationViewModel>> Update(int id, EditJobApplicationDTO editJobApplicationDTO)
        {
            if (editJobApplicationDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<JobApplicationViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var jobApplication = await _unitOfWork.JobApplicationRepository.GetByIdAsync(id);
            if (jobApplication == null)
            {
                return ApiResponseHelper.Error<JobApplicationViewModel>(ErrorCode.NotFound, "Job Application not found");
            }

            editJobApplicationDTO.UpdateEntity(jobApplication);
            _unitOfWork.JobApplicationRepository.Update(jobApplication);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(JobApplicationViewModel.GetViewModel(jobApplication), "Updated successfully");
        }


        [HttpDelete("{id:int}")]
        public async Task<ApiResponse<bool>> SoftDelete(int id)
        {
            var jobApplication = await _unitOfWork.JobApplicationRepository.GetByIdAsync(id);
            if (jobApplication == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "job not found.");
            }
            if (jobApplication.Status != JobApplicationStatus.UnderReview)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "We are sorry you can't Withdrawn you application now.");
            }
            jobApplication.Status = JobApplicationStatus.Withdrawn;
            jobApplication.DeletedAt = DateTime.UtcNow;
            jobApplication.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.JobApplicationRepository.Update(jobApplication);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Job Application is Withdrawn successfully.");
        }
    }
}
