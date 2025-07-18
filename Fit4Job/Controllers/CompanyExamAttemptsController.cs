namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyExamAttemptsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyExamAttemptService _companyExamAttemptService;

        public CompanyExamAttemptsController(UserManager<ApplicationUser> userManager, ICompanyExamAttemptService companyExamAttemptService)
        {
            _userManager = userManager;
            _companyExamAttemptService = companyExamAttemptService;
        }
        /* ****************************************** Endpoints ****************************************** */

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<CompanyExamAttemptViewModel>>> GetAll()
        {
            return await _companyExamAttemptService.GetAll();
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResponse<CompanyExamAttemptViewModel>> GetById(int id)
        {
            return await _companyExamAttemptService.GetById(id);
        }

        [HttpGet("exam/{examId:int}")]
        public async Task<ApiResponse<IEnumerable<CompanyExamAttemptViewModel>>> GetAllByExam(int examId)
        {
            return await _companyExamAttemptService.GetAllByExam(examId);
        }

        [HttpPost]
        public async Task<ApiResponse<CompanyExamAttemptViewModel>> Create(CreateCompanyExamAttemptDTO examAttemptDTO)
        {
            if (examAttemptDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyExamAttemptViewModel>(ErrorCode.BadRequest, "Invalid data");
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return ApiResponseHelper.Error<CompanyExamAttemptViewModel>(ErrorCode.Unauthorized, "User not found or unauthorized");
            }

            return await _companyExamAttemptService.Create(examAttemptDTO, user);
        }
    }
}