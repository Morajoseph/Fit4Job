using Fit4Job.DTOs.JobApplicationsDTOs;
using Fit4Job.ViewModels.JobApplicationsViewModels;
using Microsoft.Data.SqlClient;

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

        [HttpGet("job/{JobId:int}/user/{userId:int}")]
        public async Task<ApiResponse<JobApplicationViewModel>> GetJobApplicationForUser(int JobId, int userId)
        {
            var jobApplication = await _unitOfWork.JobApplicationRepository.GetByUserAndJobAsync(JobId, userId);
            if (jobApplication == null)
            {
                return ApiResponseHelper.Error<JobApplicationViewModel>(ErrorCode.NotFound, "Job Application not found for this user and job.");
            }
            return ApiResponseHelper.Success(JobApplicationViewModel.GetViewModel(jobApplication), "Job Application found successfully.");
        }
        
        [HttpPost]
        public async Task<ApiResponse<JobApplicationViewModel>> Create(CreateJobApplicationDTO createJobApplicationDTO)
        {
            if (createJobApplicationDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<JobApplicationViewModel>(ErrorCode.BadRequest, "Invalid data");
            }
            
            var job = await _unitOfWork.JobRepository.GetByIdAsync(createJobApplicationDTO.JobId);
            if (job == null)
            {
                return ApiResponseHelper.Error<JobApplicationViewModel>(ErrorCode.NotFound, "Job not found or invalid ID");
            }

            var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(createJobApplicationDTO.UserId);
            if(user == null)
            {
                return ApiResponseHelper.Error<JobApplicationViewModel>(ErrorCode.NotFound, "User not found or invalid ID");
            }

            try
            {
                var jobApplication = createJobApplicationDTO.ToEntity();
                await _unitOfWork.JobApplicationRepository.AddAsync(jobApplication);
                await _unitOfWork.CompleteAsync();
                return ApiResponseHelper.Success(JobApplicationViewModel.GetViewModel(jobApplication), "Your Job Application received successfully");
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
            {
                return ApiResponseHelper.Error<JobApplicationViewModel>(ErrorCode.Conflict, "You have already applied for this job.");
            }
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
