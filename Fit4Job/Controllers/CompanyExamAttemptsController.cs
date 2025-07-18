using Fit4Job.DTOs.CompanyExamAttemptsDTOs;
using Fit4Job.ViewModels.CompanyExamAttemptsViewModels;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyExamAttemptsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyExamAttemptsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        /* ****************************************** Endpoints ****************************************** */

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<CompanyExamAttemptViewModel>>> GetAll()
        {
            var companyExamAttempts = await _unitOfWork.CompanyExamAttemptRepository.GetAllAsync();
            var data = companyExamAttempts.Select(ea => CompanyExamAttemptViewModel.GetViewModel(ea));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResponse<CompanyExamAttemptViewModel>> GetById(int id)
        {
            var companyExamAttempt = await _unitOfWork.CompanyExamAttemptRepository.GetByIdAsync(id);
            if (companyExamAttempt == null)
            {
                return ApiResponseHelper.Error<CompanyExamAttemptViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            return ApiResponseHelper.Success(CompanyExamAttemptViewModel.GetViewModel(companyExamAttempt));
        }

        [HttpGet("exam/{examId:int}")]
        public async Task<ApiResponse<IEnumerable<CompanyExamAttemptViewModel>>> GetAllByExam(int examId)
        {
            var companyExamAttempts = await _unitOfWork.CompanyExamAttemptRepository.GetAttemptsByExamIdAsync(examId);
            if (companyExamAttempts == null)
            {
                return ApiResponseHelper.Error<IEnumerable<CompanyExamAttemptViewModel>>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            var data = companyExamAttempts.Select(ea => CompanyExamAttemptViewModel.GetViewModel(ea));
            return ApiResponseHelper.Success(data);
        }

        [HttpPost]
        public async Task<ApiResponse<CompanyExamAttemptViewModel>> Create(CreateCompanyExamAttemptDTO examAttemptDTO)
        {
            if (examAttemptDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyExamAttemptViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || examAttemptDTO.UserId != user.Id)
            {
                return ApiResponseHelper.Error<CompanyExamAttemptViewModel>(ErrorCode.Unauthorized, "User not found or unauthorized");
            }
            var exam = await _unitOfWork.CompanyExamRepository.GetByIdAsync(examAttemptDTO.ExamId);
            if (exam == null)
            {
                return ApiResponseHelper.Error<CompanyExamAttemptViewModel>(ErrorCode.NotFound, "Exam not found or invalid ID");
            }

            var jobApplication = await _unitOfWork.JobApplicationRepository.GetByIdAsync(examAttemptDTO.JobApplicationId);
            if (jobApplication == null)
            {
                return ApiResponseHelper.Error<CompanyExamAttemptViewModel>(ErrorCode.NotFound, "Job Application not found or invalid ID");
            }
            else if (jobApplication.UserId != user.Id)
            {
                return ApiResponseHelper.Error<CompanyExamAttemptViewModel>(ErrorCode.Unauthorized, "You are not authorized to start an exam attempt for this job application");
            }

            // Check if an exam attempt already exists for this job application
            var existingAttempt = await _unitOfWork.CompanyExamAttemptRepository.GetByJobApplicationIdAsync(examAttemptDTO.JobApplicationId);
            if(existingAttempt != null)
            {
                return ApiResponseHelper.Error<CompanyExamAttemptViewModel>(ErrorCode.Conflict, "An exam attempt already exists for this job application");
            }

            var examAttempt = examAttemptDTO.ToEntity();
            await _unitOfWork.CompanyExamAttemptRepository.AddAsync(examAttempt);
            jobApplication.ExamAttemptId = examAttempt.Id; // Link the exam attempt to the job application
            jobApplication.Status = JobApplicationStatus.UnderReview; // Update the job application status
            _unitOfWork.JobApplicationRepository.Update(jobApplication);
            
            await _unitOfWork.CompleteAsync();
            return ApiResponseHelper.Success(CompanyExamAttemptViewModel.GetViewModel(examAttempt), "Exam Attempt Created successfully");
        }
    }
}