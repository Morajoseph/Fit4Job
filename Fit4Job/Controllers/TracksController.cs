using Fit4Job.DTOs.TracksDTOs;
using Fit4Job.ViewModels.TracksViewModels;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public TracksController(IUnitOfWork unitOfWork , UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [HttpGet("Active/All")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> GetAllActive()
        {
            var activeTracks = await unitOfWork.TrackRepository.GetActiveTracksAsync();
            var data = activeTracks.Select(t => TrackViewModel.GetViewModel(t));
            return  ApiResponseHelper.Success(data);
        }

        [HttpGet("Id/{id:int}")]
        public async Task<ApiResponse<TrackViewModel>> GetById(int id)
        {
            var track = await unitOfWork.TrackRepository.GetByIdAsync(id);
            if (track == null)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            return ApiResponseHelper.Success(new TrackViewModel(track));
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<TrackViewModel>> Create(CreateTrackDTO createTrackDTO)
        {
            if (createTrackDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var adminIdString = userManager.GetUserId(User);
            int adminId = int.Parse(adminIdString);
            var track = createTrackDTO.GetTrack(adminId);


            await unitOfWork.TrackRepository.AddAsync(track);
            await unitOfWork.CompleteAsync();
            var trackViewModel = new TrackViewModel(track);

            return ApiResponseHelper.Success(trackViewModel, "Created successfully");
        }



        //Get questions for a specific track

        [HttpGet("{id}/questions")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionViewModel>>> GetQuestionsForTrack(int id)
        {
            var track = await unitOfWork.TrackRepository.GetTrackWithQuestionsAsync(id);

            if (track == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionViewModel>>(ErrorCode.NotFound, "Track not found");
            }

            var questions = track.TrackQuestions?
                .Where(q => q.DeletedAt == null)
                .Select(TrackQuestionViewModel.GetViewModel);
                

            return ApiResponseHelper.Success(questions);
        }


        //Get badges earned from this track

        [HttpGet("{id}/badges")]
        public async Task<ApiResponse<IEnumerable<BadgeViewModel>>> GetBadgesByTrackId(int id)
        {
            var badges = await unitOfWork.TrackRepository.GetBadgesByTrackIdAsync(id);

            if (badges == null || !badges.Any())
            {
                return ApiResponseHelper.Error<IEnumerable<BadgeViewModel>>(ErrorCode.NotFound, "No badges found for this track");
            }

            var data = badges.Select(BadgeViewModel.GetViewModel);
            return ApiResponseHelper.Success(data);
        }

        // Get track with category and creator details

        [HttpGet("{id}/details")]
        public async Task<ApiResponse<TrackDetailsViewModel>> GetTrackDetails(int id)
        {
            var track = await unitOfWork.TrackRepository.GetTrackWithDetailsAsync(id);

            if (track == null)
                return ApiResponseHelper.Error<TrackDetailsViewModel>(ErrorCode.NotFound, "Track not found");

            var viewModel = TrackDetailsViewModel.GetViewModel(track);
            return ApiResponseHelper.Success(viewModel);
        }

    }
}
