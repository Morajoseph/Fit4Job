namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackQuestionsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public TrackQuestionsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        /* ****************************************** Endpoints ****************************************** */



    }
}
