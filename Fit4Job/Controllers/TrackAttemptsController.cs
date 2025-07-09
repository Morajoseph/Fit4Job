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
        public async Task<ApiResponse<TrackAttemptViewModel>> Create(CreateTrackAttemptDTO createTrackAttemptDTO)
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


        //update track attempt

        [HttpPut("{id:int}")]
        public async Task<ApiResponse<TrackAttemptViewModel>> UpdateTrackAttempt(int id, EditTrackAttemptDTO editTrackAttemptDTO)
        {
            if (editTrackAttemptDTO == null)
            {
                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.BadRequest, "Invalid data ");

            }

            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.BadRequest, "Invalid data ");


            }
            if (id != editTrackAttemptDTO.AttemptId)
            {
                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.BadRequest, "ID mismatch");

            }


            var attempt = await unitOfWork.TrackAttemptRepository.GetByIdAsync(id);
            if (attempt == null)
            {
                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.NotFound, "Track attempt not found");
            }

            var userExist = await unitOfWork.ApplicationUserRepository.GetByIdAsync(editTrackAttemptDTO.UserId);
            if (userExist is null)
            {
                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.BadRequest, "Invalid user");
            }

            var trackExist = await unitOfWork.TrackRepository.GetByIdAsync(editTrackAttemptDTO.TrackId);
            if (trackExist is null)
            {
                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.BadRequest, "Invalid track");
            }

            attempt.UserId = editTrackAttemptDTO.UserId;
            attempt.TrackId = editTrackAttemptDTO.TrackId;
            attempt.Status = editTrackAttemptDTO.Status;
            attempt.SolvedQuestionsCount = editTrackAttemptDTO.SolvedQuestionsCount;
            attempt.TotalScore = editTrackAttemptDTO.TotalScore;
            attempt.ProgressPercentage = editTrackAttemptDTO.ProgressPercentage;



            try
            {
                unitOfWork.TrackAttemptRepository.Update(attempt);
                await unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {

                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.InternalServerError, "An error occurred while updating the track attempt");
            }

            var updatedVM = new TrackAttemptViewModel(attempt);
            return ApiResponseHelper.Success(updatedVM, "Track attempt Updated successfully");

        }

        [HttpPatch("{id:int}/end")]
        public async Task<ApiResponse<TrackAttemptViewModel>> EndTrackAttempt(int id)
        {
            var attempt = await unitOfWork.TrackAttemptRepository.GetByIdAsync(id);
            if (attempt == null)
            {
                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.NotFound, "Track attempt not found");
            }


            if (attempt.Status == AttemptStatus.Completed)
            {
                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.BadRequest, "Track attempt is already completed");
            }

            if (attempt.Status != AttemptStatus.InProgress)
            {
                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.BadRequest, "Only in-progress attempts can be ended");
            }


            attempt.EndTime = DateTime.UtcNow;
            attempt.Status = AttemptStatus.Completed;




            try
            {
                unitOfWork.TrackAttemptRepository.Update(attempt);
                await unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Error<TrackAttemptViewModel>(ErrorCode.InternalServerError, "An error occurred while ending the track attempt");
            }

            var updatedVM = new TrackAttemptViewModel(attempt);
            return ApiResponseHelper.Success(updatedVM, "Track attempt completed successfully");
        }

        [HttpGet("user/{userId}")]
        public async Task<ApiResponse<IEnumerable<TrackAttemptViewModel>>> GetUserTrackAttempts(int userId)
        {
            var user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackAttemptViewModel>>(ErrorCode.NotFound, "User not found");
            }

            try
            {
                var attempts = await unitOfWork.TrackAttemptRepository.GetAllAttemptsByUserAsync(userId);
                var attemptViewModels = attempts.Select(attempt => new TrackAttemptViewModel(attempt));

                return ApiResponseHelper.Success(attemptViewModels, "User track attempts retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackAttemptViewModel>>(ErrorCode.InternalServerError, "An error occurred while retrieving user track attempts");
            }
        }
    }
}
