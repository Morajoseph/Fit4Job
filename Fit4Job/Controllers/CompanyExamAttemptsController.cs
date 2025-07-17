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
            
            // Validate the exam exists
            // You can also check if the user is authorized to start an exam attempt

            var examAttempt = examAttemptDTO.ToEntity();
            await _unitOfWork.CompanyExamAttemptRepository.AddAsync(examAttempt);
            await _unitOfWork.CompleteAsync();
            return ApiResponseHelper.Success(CompanyExamAttemptViewModel.GetViewModel(examAttempt), "Created successfully");
        }
    }
}