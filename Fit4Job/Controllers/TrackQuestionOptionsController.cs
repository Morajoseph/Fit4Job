using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackQuestionOptionsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public TrackQuestionOptionsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        /* ****************************************** Endpoints ****************************************** */

    }
}
