using Fit4Job.DTOs.TrackQuestionOptionsDTOs;
using Fit4Job.ViewModels.TrackQuestionOptionsViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackQuestionOptionsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public TrackQuestionOptionsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        /* ****************************************** Endpoints ****************************************** */

        //Retrieves a single question option by its unique identifier.

        [HttpGet("{id:int}")]

        public async Task<ApiResponse<TrackQuestionOptionViewModel>>GetById(int id)
        {
            var option = await unitOfWork.TrackQuestionOptionRepository.GetByIdAsync(id);

            if (option == null || !option.IsActive)
                return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.NotFound, "Option not found");

            var viewModel = TrackQuestionOptionViewModel.GetViewModel(option);

            return ApiResponseHelper.Success(viewModel);
        }


        //Retrieves only the options that are marked as correct for a specific question ID.

        [HttpGet("correct/by-question/{questionId:int}")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionOptionViewModel>>> GetCorrectOptionsByQuestionId(int questionId)
        {

            var question = await unitOfWork.TrackQuestionRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionOptionViewModel>>(
                    ErrorCode.NotFound, "Question not found.");
            }

            var options = await unitOfWork.TrackQuestionOptionRepository.GetCorrectOptionsAsync(questionId);

            if (!options.Any())
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionOptionViewModel>>(
                    ErrorCode.NotFound, "No correct options found for this question.");
            }

            var data = options.Select(TrackQuestionOptionViewModel.GetViewModel);
            return ApiResponseHelper.Success(data);
        }

        //Creates a new question option in the system.
        
        [HttpPost("create")]

        public async Task<ApiResponse<TrackQuestionOptionViewModel>>Create(CreateTrackQuestionOptionDTO dto)
        {
            if(dto==null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.BadRequest, "invalid data");
            }


            var question = await unitOfWork.TrackQuestionRepository.GetByIdAsync(dto.QuestionId);
            if (question == null)
                return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.NotFound, "Question not found");


            var model = dto.ToModel();

            await unitOfWork.TrackQuestionOptionRepository.AddAsync(model);
            await unitOfWork.CompleteAsync();

            var viewModel = new TrackQuestionOptionViewModel(model);

            return ApiResponseHelper.Success(viewModel, "Option created successfully");
        }




    }
}
