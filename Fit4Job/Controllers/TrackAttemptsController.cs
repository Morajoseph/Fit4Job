namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackAttemptsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public TrackAttemptsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }



    }
}
