using Fit4Job.DTOs.TrackAttemptsDTOs;
using Fit4Job.ViewModels.TrackAttemptsViewModels;
using Fit4Job.ViewModels.TrackQuestionAnswerViewModel;
using Fit4Job.ViewModels.UserBadgeViewModels;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackAttemptsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public TrackAttemptsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [HttpGet("All")]
        public async Task<ApiResponse<IEnumerable<TrackAttemptViewModel>>> GetAll()
        {
            var trackAttempts = await unitOfWork.TrackAttemptRepository.GetAllAsync();
            var data = trackAttempts.Select(t => TrackAttemptViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("Id/{id:int}")]
        public async Task<ApiResponse<TrackAttemptViewModel>> GetById(int id)
        {
            var trackAttempt = await unitOfWork.TrackAttemptRepository.GetByIdAsync(id);
            if (trackAttempt == null)
            {
                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            return ApiResponseHelper.Success(new TrackAttemptViewModel(trackAttempt));
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<TrackAttemptViewModel>> Create(CreateTrackAttemptDTO createTrackAttemptDTO )
        {
            if (createTrackAttemptDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var userIdString = userManager.GetUserId(User);
            int userId = int.Parse(userIdString);
            var trackAttempt = createTrackAttemptDTO.GetTrack(userId);


            await unitOfWork.TrackAttemptRepository.AddAsync(trackAttempt);
            await unitOfWork.CompleteAsync();
            var trackAttemptViewModel = new TrackAttemptViewModel(trackAttempt);

            return ApiResponseHelper.Success(trackAttemptViewModel, "Created successfully");
        }


        [HttpGet("track/{trackId:int}")]
        public async Task<ApiResponse<IEnumerable<TrackAttemptViewModel>>> GetAllByTrack(int trackId)
        {
            var userIdString = userManager.GetUserId(User);
            int userId = int.Parse(userIdString);

            var attempts = await unitOfWork.TrackAttemptRepository.GetAllAttemptsByUserInTrackAsync(userId, trackId);
            var data = attempts.Select(t => TrackAttemptViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }


        [HttpGet("{id:int}/answers")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionAnswerViewModel>>> GetAnswers(int id)
        {
            var answers = await unitOfWork.TrackQuestionAnswerRepository.GetAllAnswersByAttemptAsync(id);
            var data = answers.Select(a => TrackQuestionAnswerViewModel.FromModel(a));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("{id:int}/badges")]
        public async Task<ApiResponse<IEnumerable<UserBadgeViewModels>>> GetBadges(int id)
        {
            var badges = await unitOfWork.UserBadgeRepository.GetBadgesByAttemptIdAsync(id);
            var data = badges.Select(b => UserBadgeViewModels.FromModel(b));
            return ApiResponseHelper.Success(data);
        }


    }
}
