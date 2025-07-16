namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyExamAttemptsController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyExamAttemptsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        /* ****************************************** Endpoints ****************************************** */


        /*
            GET /api/companyexamattempts - Get all attempts with filtering (by user, exam, status)
            GET /api/companyexamattempts/{id} - Get specific attempt details
            GET /api/companyexamattempts/exam/{examId} - Get all attempts for a specific exam
            GET /api/companyexamattempts/{id}/results - Get attempt results and score
            POST /api/companyexamattempts - Start a new exam attempt
            PUT /api/companyexamattempts/{id} - Update attempt (mainly for ending exam)
            POST /api/companyexamattempts/{id}/submit - Submit/complete an exam attempt --- Ask Yousry first
         */



    }
}
