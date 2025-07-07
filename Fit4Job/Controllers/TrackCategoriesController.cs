using Fit4Job.DTOs.TrackCategoriesDTOs;
using Fit4Job.Models;
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


        //  create track category
        [HttpPost]
        public async Task<ApiResponse<TrackCategoryViewModel>> Create([FromBody] CreateTrackCategoryDTO createTrackCategoryDTO)
        {
            if(createTrackCategoryDTO == null)
            {
                return ApiResponseHelper.Error<TrackCategoryViewModel>(ErrorCode.BadRequest, "Invalid data");

            }

            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackCategoryViewModel>(ErrorCode.BadRequest, "Invalid data");
            }
            var trackCategory = new TrackCategory()
            {
                Name = createTrackCategoryDTO.Name,
                Description = createTrackCategoryDTO.Description


            };
           
        

            await unitOfWork.TrackCategoryRepository.AddAsync(trackCategory);
            await unitOfWork.CompleteAsync();
            var createdVM = new TrackCategoryViewModel(trackCategory);
            return ApiResponseHelper.Success(createdVM, "Created successfully");

        }


        //update track category by Id

        [HttpPut("{id:int}")]
        public async Task<ApiResponse<TrackCategoryViewModel>> UpdateCategory(int id , [FromBody] EditTrackCategoryDTO editTrackCategoryDTO)
        {
         
            if (editTrackCategoryDTO == null)
            {
                return ApiResponseHelper.Error<TrackCategoryViewModel>(ErrorCode.BadRequest, "Request body is required");
            }


            if (!ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackCategoryViewModel>(ErrorCode.BadRequest, "Invalid data");
            }
            var oldCategory =  await unitOfWork.TrackCategoryRepository.GetByIdAsync(id);
           
            if (oldCategory == null)
            {
                return ApiResponseHelper.Error<TrackCategoryViewModel>(ErrorCode.NotFound, "Category not found");
            }

          
            oldCategory.Name= editTrackCategoryDTO.Name;
            oldCategory.Description= editTrackCategoryDTO.Description;

             unitOfWork.TrackCategoryRepository.Update(oldCategory);
             await unitOfWork.CompleteAsync();


            var updatedVM = new TrackCategoryViewModel(oldCategory);
            return ApiResponseHelper.Success(updatedVM,"updated successfully");

        }


        // DELETE: Soft Delete a track category
        [HttpDelete("{id:int}")]
        public async Task<ApiResponse<string>> SoftDelete(int id)
        {
            var category = await unitOfWork.TrackCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.NotFound, "Category not found");
            }

            if (category.DeletedAt != null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.BadRequest, "Category is already deleted");
            }

            category.DeletedAt = DateTime.UtcNow;
            unitOfWork.TrackCategoryRepository.Update(category);
            await unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success("Category deleted successfully");
        }


        // PATCH: Restore a soft-deleted track category
        [HttpPatch("{id:int}/restore")]
        public async Task<ApiResponse<string>> Restore(int id)
        {
            var category = await unitOfWork.TrackCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.NotFound, "Category not found");
            }

            if (category.DeletedAt == null)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.BadRequest, "Category is not deleted");
            }

            category.DeletedAt = null;
            unitOfWork.TrackCategoryRepository.Update(category);
            await unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success("Category restored successfully");
        }


        // Get all track categories
        [HttpGet("all")]
        public async Task<ApiResponse<IEnumerable<TrackCategoryViewModel>>> GetAllTrackCategories()
        {
            var categories = await unitOfWork.TrackCategoryRepository.GetAllAsync();
            var data = categories.Select(c => TrackCategoryViewModel.GetViewModel(c));
            return ApiResponseHelper.Success(data);
        }


        // Get all track categories by search name,status

        [HttpGet("search/{keyword}/{isActive}")]
        public async Task<ApiResponse<IEnumerable<TrackCategoryViewModel>>> Search(string keyword, bool isActive)
        {
            var categories = await unitOfWork.TrackCategoryRepository.SearchByNameAndStatusAsync(keyword,isActive);
            var data = categories.Select(c => TrackCategoryViewModel.GetViewModel(c));
            return ApiResponseHelper.Success(data);

        }

    }
}