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

        public TracksController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [HttpGet("Active/All")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> GetAllActive()
        {
            var activeTracks = await unitOfWork.TrackRepository.GetActiveTracksAsync();
            var data = activeTracks.Select(t => TrackViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
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

        //update track
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

            var oldTrack = await unitOfWork.TrackRepository.GetByIdAsync(id);
            if (oldTrack == null)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.NotFound, "Track not found");
            }

            if (id != editTrackDTO.TrackId)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.BadRequest, "ID mismatch");
            }

            oldTrack.CategoryId = editTrackDTO.CategoryId;
            oldTrack.Name = editTrackDTO.Name;
            oldTrack.Description = editTrackDTO.Description;
            oldTrack.IsPremium = editTrackDTO.IsPremium;
            oldTrack.Price = editTrackDTO.Price;
            oldTrack.TrackQuestionsCount = editTrackDTO.TrackQuestionsCount;
            oldTrack.TrackTotalScore = editTrackDTO.TrackTotalScore;
            oldTrack.UpdatedAt = DateTime.Now;

            try
            {
                unitOfWork.TrackRepository.Update(oldTrack);
                await unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
              
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.InternalServerError, "An error occurred while updating the track");
            }

            var updatedVM = new TrackViewModel(oldTrack);
            return ApiResponseHelper.Success(updatedVM, "Track Updated successfully");
        }


        // DELETE: Soft Delete a track 
        [HttpDelete("{id:int}")]

        public async Task<ApiResponse<string>> SoftDelete(int id)
        {
            var track = await unitOfWork.TrackRepository.GetByIdAsync(id);
            if(track == null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.NotFound, "Track not found");
            }
            if(track.DeletedAt != null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.BadRequest, "Track is already deleted");

            }
            track.DeletedAt = DateTime.Now;
            track.UpdatedAt = DateTime.Now;


            unitOfWork.TrackRepository.Update(track);
            await unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(" Track deleted successfully ");

        }

        // PATCH: Restore a soft-deleted track

        [HttpPatch("{id:int}/restore")]

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
            track.UpdatedAt = DateTime.Now;

            try
            {
                unitOfWork.TrackRepository.Update(track);
                await unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {

                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.InternalServerError, "An error occurred while restoring the track");
            }

            var restoredVM = new TrackViewModel(track);

            return ApiResponseHelper.Success(restoredVM, "Track restored successfully");


        }






    }
} 
