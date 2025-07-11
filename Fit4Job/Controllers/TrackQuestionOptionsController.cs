using Fit4Job.DTOs.TrackQuestionOptionsDTOs;
using Fit4Job.ViewModels.TrackQuestionOptionsViewModels;

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

        // 1 - Retrieves a single question option by its unique identifier.
        [HttpGet("{id:int}")]
        public async Task<ApiResponse<TrackQuestionOptionViewModel>> GetById(int id)
        {
            var option = await unitOfWork.TrackQuestionOptionRepository.GetByIdAsync(id);

            if (option == null || !option.IsActive)
                return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.NotFound, "Option not found");

            var viewModel = TrackQuestionOptionViewModel.GetViewModel(option);
            return ApiResponseHelper.Success(viewModel);
        }

        // 2 - Retrieves only the options that are marked as correct for a specific question ID.
        [HttpGet("correct/by-question/{questionId:int}")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionOptionViewModel>>> GetCorrectOptionsByQuestionId(int id)
        {
            var question = await unitOfWork.TrackQuestionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionOptionViewModel>>(ErrorCode.NotFound, "Question not found.");
            }

            var options = await unitOfWork.TrackQuestionOptionRepository.GetCorrectOptionsAsync(id);
            var data = options.Select(o => TrackQuestionOptionViewModel.GetViewModel(o));
            return ApiResponseHelper.Success(data);
        }

        // 3 -  Creates a new question option in the system.
        [HttpPost]
        public async Task<ApiResponse<TrackQuestionOptionViewModel>> Create(CreateTrackQuestionOptionDTO dto)
        {
            if (dto == null || !ModelState.IsValid)
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

        // 4 - Updates an existing question option identified by its ID with new data.
        [HttpPut("{id:int}")]
        public async Task<ApiResponse<TrackQuestionOptionViewModel>> Update(int id, EditTrackQuestionOptionDTO editOptionDTO)
        {
            if (editOptionDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.BadRequest, "Invalid data");
            }


            var questionOption = await unitOfWork.TrackQuestionOptionRepository.GetByIdAsync(id);
            if (questionOption == null)
            {
                return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.NotFound, "Not Found Question Option for this ID");
            }

            editOptionDTO.UpdateModel(questionOption);
            unitOfWork.TrackQuestionOptionRepository.Update(questionOption);
            await unitOfWork.CompleteAsync();

            var updatedVM = new TrackQuestionOptionViewModel(questionOption);
            return ApiResponseHelper.Success(updatedVM, "Question option Updated successfully");
        }

        // 5 -  Performs a soft delete on a question option
        [HttpDelete("delete/{id:int}")]
        public async Task<ApiResponse<bool>> SoftDelete(int id)
        {
            var questionOption = await unitOfWork.TrackQuestionOptionRepository.GetByIdAsync(id);
            if (questionOption == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "Question Option not found");
            }
            if (questionOption.DeletedAt != null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Track question option is already deleted");
            }

            questionOption.DeletedAt = DateTime.Now;
            questionOption.UpdatedAt = DateTime.Now;


            unitOfWork.TrackQuestionOptionRepository.Update(questionOption);
            await unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Track question option deleted successfully.");
        }

        // 6 -  Retrieves all question options associated with a specific question ID 
        [HttpGet("by-question/{id:int}")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionOptionViewModel>>> GetOptionsByQuestionId(int id)
        {
            var question = await unitOfWork.TrackQuestionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionOptionViewModel>>(ErrorCode.NotFound, "Question not found.");
            }

            var options = await unitOfWork.TrackQuestionOptionRepository.GetOptionsByQuestionIdAsync(id);
            
            var data = options.Select(o => TrackQuestionOptionViewModel.GetViewModel(o));
            return ApiResponseHelper.Success(data);
        }

        // 7 - get all active options for question
        [HttpGet("active/by-question/{questionId:int}")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionOptionViewModel>>> GetActiveOptionsByQuestionId(int questionId)
        {
            if (questionId <= 0)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionOptionViewModel>>(ErrorCode.BadRequest, "Invalid question ID");
            }

            var question = await unitOfWork.TrackRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionOptionViewModel>>(ErrorCode.NotFound, "question not found");
            }

            var activeOptions = await unitOfWork.TrackQuestionOptionRepository.GetActiveOptionsByQuestionIdAsync(questionId);
            var data = activeOptions.Select(q => TrackQuestionOptionViewModel.GetViewModel(q));

            return ApiResponseHelper.Success(data);
        }

        // 8 - Restores a soft-deleted question
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
                    return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.NotFound, "Question option not found");
                }

                if (option.DeletedAt == null)
                {
                    return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.BadRequest, "Question option is not deleted");
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

                return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.InternalServerError, "An error occurred while restoring the question option");
            }
        }

        // 9 - Marks a specific question option as correct.
        [HttpPatch("{id:int}/mark-correct")]
        public async Task<ApiResponse<TrackQuestionOptionViewModel>> MarkQuestionOptionCorrect(int id)
        {
            var option = await unitOfWork.TrackQuestionOptionRepository.GetByIdAsync(id);
            if(option == null )
            {
                return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.NotFound, "Option not found");
            }

            if(option.IsCorrect)
            {
                return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.BadRequest, "Option Is Alrady Correct");
            }

            option.IsCorrect = true;
            option.UpdatedAt = DateTime.UtcNow;
            unitOfWork.TrackQuestionOptionRepository.Update(option);
            await unitOfWork.CompleteAsync();

            var optionViewModel = TrackQuestionOptionViewModel.GetViewModel(option);
            return ApiResponseHelper.Success(optionViewModel);
        }

        // 10 -Marks a specific question option as incorrect
        [HttpPatch("{id:int}/mark-incorrect")]
        public async Task<ApiResponse<TrackQuestionOptionViewModel>> MarkQuestionOptionIncorrect(int id)
        {
            var option = await unitOfWork.TrackQuestionOptionRepository.GetByIdAsync(id);
            if (option == null)
            {
                return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.NotFound, "Option not found");
            }

            if (!option.IsCorrect)
            {
                return ApiResponseHelper.Error<TrackQuestionOptionViewModel>(ErrorCode.BadRequest, "Option Is Alrady Incorrect");
            }

            option.IsCorrect = false;
            option.UpdatedAt = DateTime.UtcNow;
            unitOfWork.TrackQuestionOptionRepository.Update(option);
            await unitOfWork.CompleteAsync();

            var optionViewModel = TrackQuestionOptionViewModel.GetViewModel(option);
            return ApiResponseHelper.Success(optionViewModel);
        }
    }
}
