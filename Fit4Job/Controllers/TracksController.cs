using Fit4Job.DTOs.TracksDTOs;
using Fit4Job.ViewModels.BadgesViewModels;
using Fit4Job.ViewModels.ComplexViewModels;
using Fit4Job.ViewModels.TrackQuestionsViewModels;
using Fit4Job.ViewModels.TracksViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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

        [HttpGet("all/active")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> GetAllActive()
        {
            var activeTracks = await unitOfWork.TrackRepository.GetActiveTracksAsync();
            var data = activeTracks.Select(t => TrackViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
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


        [HttpGet("all")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> GetAllIncludingDeleted()
        {
            var allTracks = await unitOfWork.TrackRepository.GetAllTracksIncludingDeletedAsync();
            var data = allTracks.Select(t => TrackViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("search")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> Search([FromQuery] TrackSearchDTO searchDTO)
        {
            var tracks = await unitOfWork.TrackRepository.SearchTracksAsync(searchDTO);
            var data = tracks.Select(t => TrackViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("category/{categoryId:int}")]
        public async Task<ApiResponse<IEnumerable<TrackViewModel>>> GetByCategory(int categoryId)
        {
            if (categoryId <= 0)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackViewModel>>(ErrorCode.BadRequest, "Invalid CategoryId");
            }

            var tracks = await unitOfWork.TrackRepository.GetAllTracksByCategoryIdWithQuestionsAsync(categoryId);
            //var data = tracks.Select(t => TrackViewModel.GetViewModel(t));

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

        //Get questions for a specific 
        [HttpGet("questions/{id:int}")]
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

        // Get track with category and creator details
        [HttpGet("details/{id:int}")]
        public async Task<ApiResponse<TrackDetailsViewModel>> GetTrackDetails(int id)
        {
            var track = await unitOfWork.TrackRepository.GetTrackWithDetailsAsync(id);

            if (track == null)
                return ApiResponseHelper.Error<TrackDetailsViewModel>(ErrorCode.NotFound, "Track not found");

            var viewModel = TrackDetailsViewModel.GetViewModel(track);
            return ApiResponseHelper.Success(viewModel);
        }



        [Authorize]
        [HttpPost("Create")]
        public async Task<ApiResponse<TrackViewModel>> Create(CreateTrackDTO createTrackDTO)
        {
            if (createTrackDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            ApplicationUser? admin = await userManager.GetUserAsync(User);
            if(admin == null)
            {
                return ApiResponseHelper.Error<TrackViewModel>(ErrorCode.Unauthorized, "Unauthorized");
            }
            var track = createTrackDTO.GetTrack(admin.Id);
            await unitOfWork.TrackRepository.AddAsync(track);
            await unitOfWork.CompleteAsync();
            var trackViewModel = new TrackViewModel(track);

            return ApiResponseHelper.Success(trackViewModel, "Created successfully");
        }

        //update track
        [HttpPut("update/{id:int}")]
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
        [HttpDelete("delete/{id:int}")]
        public async Task<ApiResponse<string>> SoftDelete(int id)
        {
            var track = await unitOfWork.TrackRepository.GetByIdAsync(id);
            if (track == null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.NotFound, "Track not found");
            }
            if (track.DeletedAt != null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.BadRequest, "Track is already deleted");

            }
            track.DeletedAt = DateTime.Now;
            track.UpdatedAt = DateTime.Now;


            unitOfWork.TrackRepository.Update(track);
            await unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(" Track deleted successfully ");

        }

        // PATCH: Restore a soft-deleted 
        [HttpPatch("restore/{id:int}")]
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

        //Get badges earned from this track
        //[HttpGet("badges/{id:int}")]
        //public async Task<ApiResponse<IEnumerable<BadgeViewModel>>> GetBadgesByTrackId(int id)
        //{
        //    var badges = await unitOfWork.TrackRepository.GetBadgesByTrackIdAsync(id);

        //    if (badges == null || !badges.Any())
        //    {
        //        return ApiResponseHelper.Error<IEnumerable<BadgeViewModel>>(ErrorCode.NotFound, "No badges found for this track");
        //    }

        //    var data = badges.Select(BadgeViewModel.GetViewModel);
        //    return ApiResponseHelper.Success(data);
        //}



    }
}
