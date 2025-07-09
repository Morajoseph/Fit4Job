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


    }
}
