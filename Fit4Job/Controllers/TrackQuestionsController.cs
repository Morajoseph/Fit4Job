using Fit4Job.ViewModels.TracksViewModels;

using Fit4Job.Enums;
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

        //Get all track questions.

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<TrackQuestionViewModel>>> GetAllTrackQuestions()
        {
            var questions = await unitOfWork.TrackQuestionRepository.GetAllAsync();
            var data = questions.Select(TrackQuestionViewModel.GetViewModel);
            return ApiResponseHelper.Success(data);
        }


        //Get a question by ID.

        [HttpGet("{id:int}")]
        public async Task<ApiResponse<TrackQuestionViewModel>> GetById(int id)
        {
            var question = await unitOfWork.TrackQuestionRepository.GetByIdAsync(id);

            if (question == null)
                return ApiResponseHelper.Error<TrackQuestionViewModel>(ErrorCode.NotFound, "Question not found");

            var viewModel = TrackQuestionViewModel.GetViewModel(question);

            return ApiResponseHelper.Success(viewModel);
        }
        // track question soft delete

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


        [HttpGet("track/{trackId:int}")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionViewModel>>>GetQuestionsByTrackId(int trackId)

        {
            var track = await unitOfWork.TrackRepository.GetByIdAsync(trackId);
            if(track == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionViewModel>>(ErrorCode.BadRequest, "Invalid trackId");

            }

            var Questions = await unitOfWork.TrackQuestionRepository.GetQuestionsByTrackIdAsync(trackId);


            var data = Questions.Select(q => TrackQuestionViewModel.GetViewModel(q));
            return ApiResponseHelper.Success(data);


        }


        //=============================================================


        [HttpGet("filter")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionViewModel>>> GetQuestionByTypeAndLevel(int trackId, QuestionType questionType, QuestionLevel questionLevel)
        {
            if (trackId <= 0)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionViewModel>>(
                    ErrorCode.BadRequest,
                    "Invalid trackId"
                );
            }

            var track = await unitOfWork.TrackRepository.GetByIdAsync(trackId);
            if (track == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionViewModel>>( ErrorCode.NotFound,"Track not found");
            }

            var questions = await unitOfWork.TrackQuestionRepository.GetQuestionsByTrackIdAsync(trackId);

            var filteredQuestions = questions.Where(q =>q.DeletedAt == null && q.QuestionType == questionType &&  q.QuestionLevel == questionLevel);

           
            var data = filteredQuestions.Select(q => TrackQuestionViewModel.GetViewModel(q));

            return ApiResponseHelper.Success(data);
        

    }



    }
}
