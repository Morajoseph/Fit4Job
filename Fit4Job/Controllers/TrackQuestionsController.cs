using Fit4Job.DTOs.TrackQuestionsDTOs;
using Fit4Job.DTOs.TracksDTOs;
using Fit4Job.ViewModels.TracksViewModels;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackQuestionsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public TrackQuestionsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        /* ****************************************** Endpoints ****************************************** */

        // 1 - Get all track questions.
        [HttpGet]
        public async Task<ApiResponse<IEnumerable<TrackQuestionViewModel>>> GetAllTrackQuestions()
        {
            var questions = await unitOfWork.TrackQuestionRepository.GetAllAsync();
            var data = questions.Select(TrackQuestionViewModel.GetViewModel);
            return ApiResponseHelper.Success(data);
        }


        // 2 - Get a question by ID.
        [HttpGet("{id:int}")]
        public async Task<ApiResponse<TrackQuestionViewModel>> GetById(int id)
        {
            var question = await unitOfWork.TrackQuestionRepository.GetByIdAsync(id);

            if (question == null)
                return ApiResponseHelper.Error<TrackQuestionViewModel>(ErrorCode.NotFound, "Question not found");

            var viewModel = TrackQuestionViewModel.GetViewModel(question);

            return ApiResponseHelper.Success(viewModel);
        }


        // 3 - Get only active questions for a track.
        [HttpGet("Active/ByTrack/{id:int}")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionViewModel>>> GetActiveByTrackId(int trackId)
        {
            var track = await unitOfWork.TrackRepository.GetByIdAsync(trackId);
            if (track == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionViewModel>>(ErrorCode.BadRequest, "Invalid trackId");
            }
            var activeQuestions = await unitOfWork.TrackQuestionRepository.GetActiveByTrackIdAsync(trackId);
            var data = activeQuestions.Select(q => TrackQuestionViewModel.GetViewModel(q));
            return ApiResponseHelper.Success(data);
        }

        // 4 - Create a new track question.

        [HttpPost]
        public async Task<ApiResponse<TrackQuestionViewModel>> Create(CreateTrackQuestionDTO createTrackQuestionDTO)
        {
            if (createTrackQuestionDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackQuestionViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var trackQuestion = createTrackQuestionDTO.GetTrackQuestion();


            await unitOfWork.TrackQuestionRepository.AddAsync(trackQuestion);
            await unitOfWork.CompleteAsync();
            var trackQuestionViewModel = new TrackQuestionViewModel(trackQuestion);

            return ApiResponseHelper.Success(trackQuestionViewModel, "Created successfully");
        }

        // 5 - Update an existing question.
        [HttpPut("Update/{id:int}")]
        public async Task<ApiResponse<TrackQuestionViewModel>> UpdateTracke(int id, EditTrackQuestionDTO editTrackQuestionDTO)
        {
            if (editTrackQuestionDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackQuestionViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var trackQuestion = await unitOfWork.TrackQuestionRepository.GetByIdAsync(id);
            if (trackQuestion == null)
            {
                return ApiResponseHelper.Error<TrackQuestionViewModel>(ErrorCode.NotFound, "Track not found");
            }

            if (id != trackQuestion.Id)
            {
                return ApiResponseHelper.Error<TrackQuestionViewModel>(ErrorCode.BadRequest, "ID mismatch");
            }

            editTrackQuestionDTO.UpdateTrackQuestion(trackQuestion);
            try
            {
                unitOfWork.TrackQuestionRepository.Update(trackQuestion);
                await unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Error<TrackQuestionViewModel>(ErrorCode.InternalServerError, "An error occurred while updating the track");
            }

            var updatedViewModel = new TrackQuestionViewModel(trackQuestion);
            return ApiResponseHelper.Success(updatedViewModel, "Track Updated successfully");
        }



        // 6 - track question soft delete
        [HttpDelete("Delete/{id:int}")]

        public async Task<ApiResponse<string>> SoftDeleteQuestion(int id)
        {
            var trackQuestion = await unitOfWork.TrackQuestionRepository.GetByIdAsync(id);
            if (trackQuestion == null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.NotFound, "Question not found");
            }
            if (trackQuestion.DeletedAt != null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.BadRequest, "Question is already deleted");

            }
            trackQuestion.DeletedAt = DateTime.UtcNow;
            trackQuestion.UpdatedAt = DateTime.UtcNow;


            unitOfWork.TrackQuestionRepository.Update(trackQuestion);
            await unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(" Question deleted successfully ");

        }

        //=======================================================================

        // 7 - Get all questions for a specific track.
        [HttpGet("track/{trackId:int}")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionViewModel>>> GetQuestionsByTrackId(int trackId)
        {
            var track = await unitOfWork.TrackRepository.GetByIdAsync(trackId);
            if (track == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionViewModel>>(ErrorCode.BadRequest, "Invalid trackId");
            }

            var Questions = await unitOfWork.TrackQuestionRepository.GetQuestionsByTrackIdAsync(trackId);
            var data = Questions.Select(q => TrackQuestionViewModel.GetViewModel(q));
            return ApiResponseHelper.Success(data);
        }


        //=============================================================

        // 8 -  Get questions using query params (type, level, etc.).
        [HttpGet("filter")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionViewModel>>> GetQuestionByTypeAndLevel(int trackId, QuestionType questionType, QuestionLevel questionLevel)
        {
            if (trackId <= 0)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionViewModel>>(ErrorCode.BadRequest, "Invalid trackId");
            }

            var track = await unitOfWork.TrackRepository.GetByIdAsync(trackId);
            if (track == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionViewModel>>(ErrorCode.NotFound, "Track not found");
            }

            var questions = await unitOfWork.TrackQuestionRepository.GetActiveByTrackIdAsync(trackId);
            var filteredQuestions = questions
                .Where(q => q.QuestionType == questionType && q.QuestionLevel == questionLevel);
            var data = filteredQuestions.Select(q => TrackQuestionViewModel.GetViewModel(q));
            return ApiResponseHelper.Success(data);
        }
    }
}
