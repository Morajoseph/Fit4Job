namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackCategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public TrackCategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


    }
}