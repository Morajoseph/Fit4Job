using Fit4Job.DTOs.TrackQuestionOptionsDTOs;
using Fit4Job.Models;
using Fit4Job.ViewModels.TrackQuestionOptionsViewModels;
using Fit4Job.ViewModels.TracksViewModels;
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



        //get all active options for question

        [HttpGet("question/{questionId:int}")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionOptionViewModel>>>GetActiveOptionsByQuestionId(int questionId)

        {
            if (questionId <= 0)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionOptionViewModel>>(
                    ErrorCode.BadRequest,
                    "Invalid question ID"
                );
            }

            var question = await unitOfWork.TrackRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionOptionViewModel>>(ErrorCode.NotFound, "question not found");
            }


            var options = await unitOfWork.TrackQuestionOptionRepository.GetOptionsByQuestionIdAsync(questionId);
            var activeOptions = options.Where(o => o.IsActive).ToList();

            var data = activeOptions.Select(q => TrackQuestionOptionViewModel.GetViewModel(q));

            return ApiResponseHelper.Success(data);

        }

        [HttpPatch("restore/{id:int}")]
        public async Task<ApiResponse<TrackQuestionOptionViewModel>> RestoreQuestionOption(int id)
        {
            try
            {
           
                if (id <= 0)
                {
                    return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(
                        ErrorCode.BadRequest,
                        "Invalid option ID"
                    );
                }
                var option = await unitOfWork.TrackQuestionOptionRepository.GetByIdAsync(id);

                if (option == null)
                {
                    return ApiResponseHelper.Error<TrackQuestionOptionViewModel>( ErrorCode.NotFound,"Question option not found");
                }

                if (option.DeletedAt == null)
                {
                    return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.BadRequest,"Question option is not deleted");
                }

             
                var question = await unitOfWork.TrackQuestionRepository.GetByIdAsync(option.QuestionId);
                if (question == null || question.DeletedAt != null)
                {
                    return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.BadRequest, "Cannot restore option for a deleted or non-existent question");
                }

             
                option.DeletedAt = null;
                option.UpdatedAt = DateTime.UtcNow;

                unitOfWork.TrackQuestionOptionRepository.Update(option);
                await unitOfWork.CompleteAsync();

         
                var restoredVM = TrackQuestionOptionViewModel.GetViewModel(option);
                return ApiResponseHelper.Success(restoredVM);
            }
            catch (Exception ex)
            {
           
                return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.InternalServerError,"An error occurred while restoring the question option" );
            }
        }



    }
}
