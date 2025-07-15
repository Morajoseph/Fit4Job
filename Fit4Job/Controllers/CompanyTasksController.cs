using System.IO.Pipelines;
using System.Threading.Tasks;
using Fit4Job.DTOs.CompanyTasksDTOs;
using Fit4Job.DTOs.JobsDTOs;
using Fit4Job.Models;
using Fit4Job.ViewModels.CompanyTasksViewModels;
using Fit4Job.ViewModels.JobsViewModels;
using Fit4Job.ViewModels.SkillsViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyTasksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyTasksController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }




        [HttpGet]
        public async Task<ApiResponse<IEnumerable<CompanyTaskViewModel>>> GetAllTasks()
        {
            var companyTasks = await _unitOfWork.CompanyTaskRepository.GetAllAsync();
            var data = companyTasks.Select(t => CompanyTaskViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);

        }



        [HttpGet("{id:int}")]
        public async Task<ApiResponse<CompanyTaskViewModel>> GetById(int id)
        {
            var task = await _unitOfWork.CompanyTaskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return ApiResponseHelper.Error<CompanyTaskViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");

            }

            return ApiResponseHelper.Success(new CompanyTaskViewModel(task));

        }

        [HttpPost]
        public async Task<ApiResponse<CompanyTaskViewModel>>Create(CreateCompanyTaskDTO createCompanyTaskDTO)
        {

            if (createCompanyTaskDTO == null || !ModelState.IsValid)
            {

                return ApiResponseHelper.Error<CompanyTaskViewModel>(ErrorCode.BadRequest, "Invalid data");

            }

            var task = createCompanyTaskDTO.ToEntity();
             _unitOfWork.CompanyTaskRepository.Update(task);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(CompanyTaskViewModel.GetViewModel(task), "Created successfully");


        }




        [HttpPut("{id:int}")]
        public async Task<ApiResponse<CompanyTaskViewModel>> Update(int id, EditCompanyTaskDTO editCompanyTaskDTO)
        {
            if (editCompanyTaskDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyTaskViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var task = await _unitOfWork.CompanyTaskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return ApiResponseHelper.Error<CompanyTaskViewModel>(ErrorCode.NotFound, "Task not found");
            }

            editCompanyTaskDTO.UpdateEntity(task);
            _unitOfWork.CompanyTaskRepository.Update(task);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(CompanyTaskViewModel.GetViewModel(task), "Updated successfully");
        }





        [HttpDelete("{id:int}")]
        public async Task<ApiResponse<string>> SoftDelete(int id)
        {
            var task = await _unitOfWork.CompanyTaskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.NotFound, "task not found.");
            }
            if (task.DeletedAt != null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.BadRequest, "task is already deleted.");
            }
            task.DeletedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.CompanyTaskRepository.Update(task);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success("task is deleted successfully.");
        }

       

    }
}
