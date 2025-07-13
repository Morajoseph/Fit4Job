namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackCategoriesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public TrackCategoriesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // Get all active categories (DeletedAt == null)
        [HttpGet]
        public async Task<ApiResponse<IEnumerable<TrackCategoryViewModel>>> GetAllActive()
        {
            var categories = await unitOfWork.TrackCategoryRepository.GetActiveCategoriesAsync();
            var data = categories.Select(c => TrackCategoryViewModel.GetViewModel(c));
            return ApiResponseHelper.Success(data);
        }

        // Get a single track category by Id
        [HttpGet("{id:int}")]
        public async Task<ApiResponse<TrackCategoryViewModel>> GetById(int id)
        {
            var category = await unitOfWork.TrackCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return ApiResponseHelper.Error<TrackCategoryViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            return ApiResponseHelper.Success(new TrackCategoryViewModel(category));
        }

        // Get all track categories
        [HttpGet("all")]
        public async Task<ApiResponse<IEnumerable<TrackCategoryViewModel>>> GetAllTrackCategories()
        {
            var categories = await unitOfWork.TrackCategoryRepository.GetAllAsync();
            var data = categories.Select(c => TrackCategoryViewModel.GetViewModel(c));
            return ApiResponseHelper.Success(data);
        }

        // Get all track categories by search name
        [HttpGet("search/{name}")]
        public async Task<ApiResponse<IEnumerable<TrackCategoryViewModel>>> SearchByName(string name)
        {
            var categories = await unitOfWork.TrackCategoryRepository.SearchByNameAsync(name);
            var data = categories.Select(c => TrackCategoryViewModel.GetViewModel(c));
            return ApiResponseHelper.Success(data);
        }

        // create track category
        [HttpPost]
        public async Task<ApiResponse<TrackCategoryViewModel>> Create(CreateTrackCategoryDTO createCategoryDTO)
        {
            if (createCategoryDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackCategoryViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var trackCategory = createCategoryDTO.ToEntity();
            await unitOfWork.TrackCategoryRepository.AddAsync(trackCategory);
            await unitOfWork.CompleteAsync();

            var trackCategoryViewModel = TrackCategoryViewModel.GetViewModel(trackCategory);
            return ApiResponseHelper.Success(trackCategoryViewModel, "Created successfully");
        }

        // Update track category by Id
        [HttpPut("{id:int}")]
        public async Task<ApiResponse<TrackCategoryViewModel>> UpdateCategory(int id, EditTrackCategoryDTO editTrackCategoryDTO)
        {
            if (editTrackCategoryDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackCategoryViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var category = await unitOfWork.TrackCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return ApiResponseHelper.Error<TrackCategoryViewModel>(ErrorCode.NotFound, "Category not found.");
            }

            editTrackCategoryDTO.UpdateEntity(category);
            unitOfWork.TrackCategoryRepository.Update(category);
            await unitOfWork.CompleteAsync();

            var updatedVM = new TrackCategoryViewModel(category);
            return ApiResponseHelper.Success(updatedVM, "updated successfully");
        }

        // Soft Delete a track category
        [HttpDelete("{id:int}")]
        public async Task<ApiResponse<bool>> SoftDelete(int id)
        {
            var category = await unitOfWork.TrackCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "Category not found");
            }

            if (category.DeletedAt != null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Category is already deleted");
            }

            category.DeletedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;
            unitOfWork.TrackCategoryRepository.Update(category);
            await unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Category deleted successfully");
        }

        // Restore a soft-deleted track category
        [HttpPatch("restore/{id:int}")]
        public async Task<ApiResponse<bool>> Restore(int id)
        {
            var category = await unitOfWork.TrackCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "Category not found");
            }

            if (category.DeletedAt == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Category is not deleted");
            }
            
            category.DeletedAt = null;
            category.UpdatedAt = DateTime.UtcNow;
            unitOfWork.TrackCategoryRepository.Update(category);
            await unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Category restored successfully");
        }
    }
}