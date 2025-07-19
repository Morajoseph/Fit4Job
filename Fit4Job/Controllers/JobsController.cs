using Fit4Job.DTOs.JobsDTOs;
using Fit4Job.ViewModels.JobsViewModels;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        /* ****************************************** Endpoints ****************************************** */

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<JobViewModel>>> GetAll()
        {
            var jobs = await _unitOfWork.JobRepository.GetAllAsync();
            var data = jobs.Select(t => JobViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResponse<JobViewModel>> GetById(int id)
        {
            var job = await _unitOfWork.JobRepository.GetByIdAsync(id);
            if (job == null)
            {
                return ApiResponseHelper.Error<JobViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            var user = await _userManager.GetUserAsync(User);
            var isUserApplied = false;
            if (user != null)
            {
                isUserApplied  = await _unitOfWork.JobApplicationRepository.ExistsAsync(user.Id, id);
            }
            return ApiResponseHelper.Success(JobViewModel.GetViewModel(job, isUserApplied));
        }

        [HttpGet("company/{companyId:int}")]
        public async Task<ApiResponse<IEnumerable<JobViewModel>>> GetAllByCompanyID (int companyId)
        {
            var jobs = await _unitOfWork.JobRepository.GetJobsByCompanyIdAsync(companyId);
            if (!jobs.Any())
            {
                return ApiResponseHelper.Error<IEnumerable<JobViewModel>>(ErrorCode.NotFound, "No jobs found for this company.");
            }

            
            var data = jobs.Select(t => JobViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpPost]
        public async Task<ApiResponse<JobViewModel>> Create(CreateJobDTO createJobDTO)
        {
            if (createJobDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<JobViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var job = createJobDTO.ToEntity();
            await _unitOfWork.JobRepository.AddAsync(job);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(JobViewModel.GetViewModel(job), "Created successfully");
        }

        [HttpPut("{id:int}")]
        public async Task<ApiResponse<JobViewModel>> Update(int id,EditJobDTO editJobDTO)
        {
            if (editJobDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<JobViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var job = await _unitOfWork.JobRepository.GetByIdAsync(id);
            if (job == null)
            {
                return ApiResponseHelper.Error<JobViewModel>(ErrorCode.NotFound, "Job not found");
            }

            editJobDTO.UpdateEntity(job);
            _unitOfWork.JobRepository.Update(job);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(JobViewModel.GetViewModel(job), "Updated successfully");
        }

        [HttpDelete("{id:int}")]
        public async Task<ApiResponse<string>> SoftDelete(int id)
        {
            var job = await _unitOfWork.JobRepository.GetByIdAsync(id);
            if (job == null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.NotFound, "job not found.");
            }
            if (job.DeletedAt != null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.BadRequest, "Job is already deleted.");
            }
            job.DeletedAt = DateTime.UtcNow;
            job.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.JobRepository.Update(job);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success("Job is deleted successfully.");
        }

        [HttpPatch("{id}/activate")]
        public async Task<ApiResponse<JobViewModel>> ActivateJob(int id)
        {
            var job = await _unitOfWork.JobRepository.GetByIdAsync(id);
            if (job == null)
            {
                return ApiResponseHelper.Error<JobViewModel>(ErrorCode.NotFound, "Job not found or deleted.");
            }

            if (job.DeletedAt == null)
            {
                return ApiResponseHelper.Error<JobViewModel>(ErrorCode.BadRequest, "Job is already active.");
            }

            job.DeletedAt = null;
            job.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.JobRepository.Update(job);
            await _unitOfWork.CompleteAsync();

            var jobVm = new JobViewModel(job);
            return ApiResponseHelper.Success(jobVm);
        }
    }
}
