using Fit4Job.UoW;
using Fit4Job.ViewModels.JobsViewModels;
using Microsoft.EntityFrameworkCore;

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
                return ApiResponseHelper.Error<JobViewModel>(ErrorCode.BadRequest, "Job is already deleted.");
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
