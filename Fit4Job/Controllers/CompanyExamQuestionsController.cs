using Fit4Job.DTOs.CompanyExamQuestionDTOs;
using Fit4Job.DTOs.CompanyProfileDTOs;
using Fit4Job.Models;
using Fit4Job.ViewModels.CompanyExamQuestionViewModels;
using Fit4Job.ViewModels.CompanyProfileViewModels;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyExamQuestionsController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyExamQuestionsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }



        /* ****************************************** Endpoints ****************************************** */


        [HttpGet]
        public async Task<ApiResponse<IEnumerable<CompanyExamQuestionViewModel>>> GetAllCompanyExamQuestions()
        {

            var companyExamQuestions = await _unitOfWork.CompanyExamQuestionRepository.GetAllAsync();
            var data = companyExamQuestions.Select(ceq => CompanyExamQuestionViewModel.GetViewModel(ceq));
            return ApiResponseHelper.Success(data);

        }


        [HttpGet("{id:int}")]
        public async Task<ApiResponse<CompanyExamQuestionViewModel>> GetById(int id)
        {
            var companyExamQuestion = await _unitOfWork.CompanyExamQuestionRepository.GetByIdAsync(id);
            if (companyExamQuestion == null)
            {
                return ApiResponseHelper.Error<CompanyExamQuestionViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            return ApiResponseHelper.Success(new CompanyExamQuestionViewModel(companyExamQuestion));
        }



        [HttpPost]
        public async Task<ApiResponse<CompanyExamQuestionViewModel>> Create(CreateCompanyExamQuestionDTO createCompanyExamQuestionDTO)
        {
            if (createCompanyExamQuestionDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyExamQuestionViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var companyExamQuestion = createCompanyExamQuestionDTO.ToEntity();
            await _unitOfWork.CompanyExamQuestionRepository.AddAsync(companyExamQuestion);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(CompanyExamQuestionViewModel.GetViewModel(companyExamQuestion), "Created successfully");
        }



        [HttpPut("{id:int}")]
        public async Task<ApiResponse<CompanyExamQuestionViewModel>> Update(int id, EditCompanyExamQuestionDTO editCompanyExamQuestionDTO)
        {
            if (editCompanyExamQuestionDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyExamQuestionViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var companyExamQuestion = await _unitOfWork.CompanyExamQuestionRepository.GetByIdAsync(id);
            if (companyExamQuestion == null)
            {
                return ApiResponseHelper.Error<CompanyExamQuestionViewModel>(ErrorCode.NotFound, "company exam question not found");
            }

            editCompanyExamQuestionDTO.UpdateEntity(companyExamQuestion);
            _unitOfWork.CompanyExamQuestionRepository.Update(companyExamQuestion);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(CompanyExamQuestionViewModel.GetViewModel(companyExamQuestion), "Updated successfully");
        }



    }
}
