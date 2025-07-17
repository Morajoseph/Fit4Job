using Fit4Job.DTOs.CompanyExamQuestionOptionsDTOs;
using Fit4Job.ViewModels.CompanyExamQuestionOptionsViewModels;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyExamQuestionOptionsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyExamQuestionOptionsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        /* ****************************************** Endpoints ****************************************** */

        [HttpGet("{id:int}")]
        public async Task<ApiResponse<CompanyExamQuestionOptionViewModel>> GetById(int id)
        {
            var option = await _unitOfWork.CompanyExamQuestionOptionRepository.GetByIdAsync(id);

            if (option == null || !option.IsActive)
                return ApiResponseHelper.Error<CompanyExamQuestionOptionViewModel>(ErrorCode.NotFound, "Option not found");

            return ApiResponseHelper.Success(CompanyExamQuestionOptionViewModel.GetViewModel(option));
        }
        
        [HttpGet("question/{questionId:int}")]
        public async Task<ApiResponse<IEnumerable<CompanyExamQuestionOptionViewModel>>> GetOptionsByQuestionId(int id)
        {
            var question = await _unitOfWork.CompanyExamQuestionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return ApiResponseHelper.Error<IEnumerable<CompanyExamQuestionOptionViewModel>>(ErrorCode.NotFound, "Question not found.");
            }

            var options = await _unitOfWork.CompanyExamQuestionOptionRepository.GetOptionsByQuestionIdAsync(id);

            var data = options.Select(o => CompanyExamQuestionOptionViewModel.GetViewModel(o));
            return ApiResponseHelper.Success(data);
        }

        [HttpPost]
        public async Task<ApiResponse<CompanyExamQuestionOptionViewModel>> Create(CreateCompanyExamQuestionOptionDTO dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyExamQuestionOptionViewModel>(ErrorCode.BadRequest, "invalid data");
            }


            var question = await _unitOfWork.CompanyExamQuestionRepository.GetByIdAsync(dto.QuestionId);
            if (question == null)
                return ApiResponseHelper.Error<CompanyExamQuestionOptionViewModel>(ErrorCode.NotFound, "Question not found");


            var model = dto.ToModel();

            await _unitOfWork.CompanyExamQuestionOptionRepository.AddAsync(model);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(CompanyExamQuestionOptionViewModel.GetViewModel(model), "Option created successfully");
        }

        [HttpPut("{id:int}")]
        public async Task<ApiResponse<CompanyExamQuestionOptionViewModel>> Update(int id, EditCompanyExamQuestionOptionDTO editOptionDTO)
        {
            if (editOptionDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyExamQuestionOptionViewModel>(ErrorCode.BadRequest, "Invalid data");
            }


            var questionOption = await _unitOfWork.CompanyExamQuestionOptionRepository.GetByIdAsync(id);
            if (questionOption == null)
            {
                return ApiResponseHelper.Error<CompanyExamQuestionOptionViewModel>(ErrorCode.NotFound, "Not Found Question Option for this ID");
            }

            editOptionDTO.UpdateModel(questionOption);
            _unitOfWork.CompanyExamQuestionOptionRepository.Update(questionOption);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(CompanyExamQuestionOptionViewModel.GetViewModel(questionOption), "Question option Updated successfully");
        }

        [HttpDelete("{id:int}")]
        public async Task<ApiResponse<bool>> SoftDelete(int id)
        {
            var questionOption = await _unitOfWork.CompanyExamQuestionOptionRepository.GetByIdAsync(id);
            if (questionOption == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "Question Option not found");
            }
            if (questionOption.DeletedAt != null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Exam question option is already deleted");
            }

            questionOption.DeletedAt = DateTime.UtcNow;


            _unitOfWork.CompanyExamQuestionOptionRepository.Update(questionOption);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Exam question option deleted successfully.");
        }
    }
}
