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


        [HttpGet("All")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> GetAllIncludingDeleted()
        {
            var allTracks = await unitOfWork.TrackRepository.GetAllTracksIncludingDeletedAsync();
            var data = allTracks.Select(t => TrackViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("Search")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> Search([FromQuery] TrackSearchDTO searchDTO)
        {
            var tracks = await unitOfWork.TrackRepository.SearchTracksAsync(searchDTO);
            var data = tracks.Select(t => TrackViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("Category/{categoryId:int}")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> GetByCategory(int categoryId)
        {
            if (categoryId <= 0)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackViewModel>>(ErrorCode.BadRequest, "Invalid CategoryId");
            }

            var tracks = await unitOfWork.TrackRepository.GetAllTracksByCategoryIdAsync(categoryId);
            var data = tracks.Select(t => TrackViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }



    }
}
