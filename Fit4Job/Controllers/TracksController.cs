using Fit4Job.ViewModels.ComplexViewModels;
using Fit4Job.ViewModels.TracksViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public TracksController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResponse<TrackViewModel>> GetById(int id)
        {
            var track = await unitOfWork.TrackRepository.GetByIdAsync(id);
            if (track == null)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            return ApiResponseHelper.Success(new TrackViewModel(track));
        }

        [HttpGet("details/{id:int}")]
        public async Task<ApiResponse<TrackDetailsViewModel>> GetTrackDetails(int id)
        {
            var track = await unitOfWork.TrackRepository.GetTrackWithDetailsAsync(id);

            if (track == null)
            {
                return ApiResponseHelper.Error<TrackDetailsViewModel>(ErrorCode.NotFound, "Track not found");
            }

            var viewModel = TrackDetailsViewModel.GetViewModel(track);
            return ApiResponseHelper.Success(viewModel);
        }

        [HttpGet("all")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> GetAllIncludingDeleted()
        {
            var allTracks = await unitOfWork.TrackRepository.GetAllTracksIncludingDeletedAsync();
            var data = allTracks.Select(t => TrackViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("all/active")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> GetAllActive()
        {
            var activeTracks = await unitOfWork.TrackRepository.GetActiveTracksAsync();
            var data = activeTracks.Select(t => TrackViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("search")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> Search([FromQuery] TrackSearchDTO searchDTO)
        {
            var tracks = await unitOfWork.TrackRepository.SearchTracksAsync(searchDTO);
            var data = tracks.Select(t => TrackViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("by-category/{categoryId:int}")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> GetByCategory(int categoryId)
        {
            if (categoryId <= 0)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackViewModel>>(ErrorCode.BadRequest, "Invalid CategoryId");
            }

            var tracks = await unitOfWork.TrackRepository.GetAllTracksByCategoryIdWithQuestionsAsync(categoryId);
            var data = tracks.Select(t =>
            {
                var viewModel = TrackViewModel.GetViewModel(t);
                if (t.TrackQuestions != null)
                {
                    viewModel.TrackDetails = TrackQuestionsDetailsViewModel.GetViewModel(t.TrackQuestions);
                }
                return viewModel;
            });

            return ApiResponseHelper.Success(data);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ApiResponse<TrackViewModel>> Create(CreateTrackDTO createTrackDTO)
        {
            if (createTrackDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            ApplicationUser? admin = await userManager.GetUserAsync(User);
            if (admin == null)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.Unauthorized, "Unauthorized");
            }
            var track = createTrackDTO.ToEntity(admin.Id);
            await unitOfWork.TrackRepository.AddAsync(track);
            await unitOfWork.CompleteAsync();

            var trackViewModel = new TrackViewModel(track);
            return ApiResponseHelper.Success(trackViewModel, "Created successfully");
        }

        [HttpPut("{id:int}")]
        public async Task<ApiResponse<TrackViewModel>> UpdateTracke(int id, EditTrackDTO editTrackDTO)
        {
            if (editTrackDTO == null)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.BadRequest, "Invalid data");
            }
            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var track = await unitOfWork.TrackRepository.GetByIdAsync(id);
            if (track == null)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.NotFound, "Track not found");
            }

            editTrackDTO.UpdateEntity(track);

            unitOfWork.TrackRepository.Update(track);
            await unitOfWork.CompleteAsync();


            var trackViewModel = new TrackViewModel(track);
            return ApiResponseHelper.Success(trackViewModel, "Track Updated successfully");
        }

        [HttpDelete("{id:int}")]
        public async Task<ApiResponse<string>> SoftDelete(int id)
        {
            var track = await unitOfWork.TrackRepository.GetByIdAsync(id);
            if (track == null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.NotFound, "Track not found.");
            }
            if (track.DeletedAt != null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.BadRequest, "Track is already deleted.");

            }
            track.DeletedAt = DateTime.Now;
            track.UpdatedAt = DateTime.Now;


            unitOfWork.TrackRepository.Update(track);
            await unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success("Track deleted successfully.");
        }

        [HttpPatch("{id:int}")]
        public async Task<ApiResponse<TrackViewModel>> Restore(int id)
        {
            var track = await unitOfWork.TrackRepository.GetByIdAsync(id);
            if (track == null)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.NotFound, " Track not found");
            }

            if (track.DeletedAt == null)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.BadRequest, "Track is not deleted");

            }

            track.DeletedAt = null;
            track.UpdatedAt = DateTime.UtcNow;

            unitOfWork.TrackRepository.Update(track);
            await unitOfWork.CompleteAsync();

            var trackViewModel = new TrackViewModel(track);

            return ApiResponseHelper.Success(trackViewModel, "Track restored successfully");
        }
    }
}
