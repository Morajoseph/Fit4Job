using Fit4Job.DTOs.CompanyTasksDTOs;
using Fit4Job.ViewModels.CompanyTasksViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Fit4Job.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/TaskSubmissions")]
    public class CompanyTaskSubmissionsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyTaskSubmissionsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        /* ****************************************** Endpoints ****************************************** */

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<CompanyTaskSubmissionViewModel>>> GetAll()
        {
            var taskSubmissions = await _unitOfWork.CompanyTaskSubmissionRepository.GetAllAsync();
            var data = taskSubmissions.Select(t => CompanyTaskSubmissionViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResponse<CompanyTaskSubmissionViewModel>> GetById(int id)
        {
            var taskSubmission = await _unitOfWork.CompanyTaskSubmissionRepository.GetByIdAsync(id);
            if (taskSubmission == null)
            {
                return ApiResponseHelper.Error<CompanyTaskSubmissionViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            return ApiResponseHelper.Success(new CompanyTaskSubmissionViewModel(taskSubmission));
        }

        [HttpGet("by-task/{taskId:int}")]
        public async Task<ApiResponse<IEnumerable<CompanyTaskSubmissionViewModel>>> GetByTaskId(int taskId)
        {
            var task = await _unitOfWork.CompanyTaskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                return ApiResponseHelper.Error<IEnumerable<CompanyTaskSubmissionViewModel>>(ErrorCode.NotFound, "Not Found or invalid Task ID");
            }
            var taskSubmissions = await _unitOfWork.CompanyTaskSubmissionRepository.GetSubmissionsByTaskIdAsync(taskId);
            var data = taskSubmissions.Select(t => CompanyTaskSubmissionViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("my-submissions")]
        public async Task<ApiResponse<IEnumerable<CompanyTaskSubmissionViewModel>>> MySubmissions()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return ApiResponseHelper.Error<IEnumerable<CompanyTaskSubmissionViewModel>>(ErrorCode.Unauthorized, "User not found or unauthorized");
            }
            var taskSubmissions = await _unitOfWork.CompanyTaskSubmissionRepository.GetSubmissionsByUserIdAsync(user.Id);
            var data = taskSubmissions.Select(t => CompanyTaskSubmissionViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpPost]
        public async Task<ApiResponse<CompanyTaskSubmissionViewModel>> Create(CreateTaskSubmission createTaskSubmission)
        {
            if (createTaskSubmission == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyTaskSubmissionViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var taskSubmission = createTaskSubmission.ToEntity();
            await _unitOfWork.CompanyTaskSubmissionRepository.AddAsync(taskSubmission);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(CompanyTaskSubmissionViewModel.GetViewModel(taskSubmission), "Task submited.");
        }

        [HttpPut("{id:int}")]
        public async Task<ApiResponse<CompanyTaskSubmissionViewModel>> Update(int id, EditTaskSubmission editTaskSubmission)
        {
            if (editTaskSubmission == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyTaskSubmissionViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var taskSubmission = await _unitOfWork.CompanyTaskSubmissionRepository.GetByIdAsync(id);
            if (taskSubmission == null)
            {
                return ApiResponseHelper.Error<CompanyTaskSubmissionViewModel>(ErrorCode.NotFound, "Job not found");
            }
            var user = await _userManager.GetUserAsync(User);
            if (user== null || taskSubmission.UserId != user.Id)
            {
                return ApiResponseHelper.Error<CompanyTaskSubmissionViewModel>(ErrorCode.Forbidden, "You can only edit your own submissions.");
            }

            editTaskSubmission.UpdateEntity(taskSubmission);
            _unitOfWork.CompanyTaskSubmissionRepository.Update(taskSubmission);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(CompanyTaskSubmissionViewModel.GetViewModel(taskSubmission), "Updated successfully");
        }

        [HttpDelete("{id:int}")]
        public async Task<ApiResponse<bool>> HardDelete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.Unauthorized, "User not found or unauthorized");
            }
            var taskSubmission = await _unitOfWork.CompanyTaskSubmissionRepository.GetByIdAsync(id);
            if (taskSubmission == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "task Submission not found.");
            }
            if (taskSubmission.UserId != user.Id)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.Forbidden, "You can only delete your own submissions.");
            }

            var isDeleted = await _unitOfWork.CompanyTaskSubmissionRepository.DeleteById(id);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Task Submission is deleted successfully.");
        }
    }
}
