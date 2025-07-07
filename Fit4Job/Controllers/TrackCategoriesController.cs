using Fit4Job.ViewModels.TrackCategoriesViewModels;

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

        // Get all active track categories (DeletedAt == null)
        [HttpGet]
        public async Task<ApiResponse<IEnumerable<TrackCategoryViewModel>>> GetAllActive()
        {
            var categories = await unitOfWork.TrackCategoryRepository.GetAllAsync();
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
    }
}