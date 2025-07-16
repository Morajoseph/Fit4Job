using Fit4Job.DTOs.CompanyProfileDTOs;
using Fit4Job.DTOs.JobsDTOs;
using Fit4Job.ViewModels.CompanyProfileViewModels;
using Fit4Job.ViewModels.JobsViewModels;
using Microsoft.AspNetCore.Identity;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyProfilesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyProfilesController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        /* ****************************************** Endpoints ****************************************** */


        [HttpGet]
        public async Task<ApiResponse<IEnumerable<CompanyProfileViewModel>>> GetAllCompanyProfiles()
        {

            var companyProfiles = await _unitOfWork.CompanyProfileRepository.GetAllAsync();
            var data = companyProfiles.Select(cp => CompanyProfileViewModel.GetViewModel(cp));
            return ApiResponseHelper.Success(data);

        }



        [HttpGet("{id:int}")]
        public async Task<ApiResponse<CompanyProfileViewModel>> GetById(int id)
        {
            var companyProfile = await _unitOfWork.CompanyProfileRepository.GetByIdAsync(id);
            if (companyProfile == null)
            {
                return ApiResponseHelper.Error<CompanyProfileViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            return ApiResponseHelper.Success(new CompanyProfileViewModel(companyProfile));
        }



        [HttpPost]
        public async Task<ApiResponse<CompanyProfileViewModel>> Create(CreateCompanyProfileDTO createCompanyProfileDTO)
        {
            if (createCompanyProfileDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyProfileViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var companyProfile = createCompanyProfileDTO.ToEntity();
            await _unitOfWork.CompanyProfileRepository.AddAsync(companyProfile);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(CompanyProfileViewModel.GetViewModel(companyProfile), "Created successfully");
        }



    }
}
