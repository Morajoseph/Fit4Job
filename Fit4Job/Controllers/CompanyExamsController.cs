using Fit4Job.DTOs.CompanyExamsDto;
using Fit4Job.Models;
using Fit4Job.ViewModels.CompanyExamsViewModel;
using Fit4Job.ViewModels.TracksViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.Design;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyExamsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public CompanyExamsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        //-----------Endpoint-----------

        // Get all exams with filtering options(by company, job, status, date range)

        [HttpGet("search")]
        public async Task<ApiResponse<IEnumerable<CompanyExamViewModel>>> Search([FromQuery]CompanyExamSearchDTO companyExamSearchDTO)
        {
            var exams = await unitOfWork.CompanyExamRepository.SearchCompanyExamsAsync(companyExamSearchDTO);
            var data = exams.Select(e => CompanyExamViewModel.GetViewModel(e));

            return ApiResponseHelper.Success(data);
        }



        //Get a specific exam by ID with full details

        [HttpGet("{id:int}")]

        public async Task<ApiResponse<CompanyExamViewModel>> GetCompanyExamByIDWithDetails(int id)
        {
            var companyExam = await unitOfWork.CompanyExamRepository.GetExamWithDetailsAsync(id);

            if (companyExam == null)
            {
                return ApiResponseHelper.Error<CompanyExamViewModel>(ErrorCode.NotFound, "CompanyExam not found");
            }

            var viewModel = CompanyExamViewModel.GetViewModel(companyExam);
            return ApiResponseHelper.Success(viewModel);
        }

        //Get all exams for a specific company

        [HttpGet("company/{companyId:int}")]
        public async Task<ApiResponse<IEnumerable<CompanyExamViewModel>>> GetAllCompanyExamsByCompanyId(int companyId)
        {

            var companyExams = await unitOfWork.CompanyExamRepository.GetActiveExamsByCompanyIdAsync(companyId);
            var data = companyExams.Select(t => CompanyExamViewModel.GetViewModel(t));
            return ApiResponseHelper.Success(data);
        }

        //Create a new exam

        [HttpPost("create")]
        public async Task<ApiResponse<CompanyExamViewModel>> Create(CreateCompanyExamDto createCompanyExamDto)
        {
            if (createCompanyExamDto == null || !ModelState.IsValid)
            {

                return ApiResponseHelper.Error<CompanyExamViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var companyExam = createCompanyExamDto.ToEntity();
            await unitOfWork.CompanyExamRepository.AddAsync(companyExam);
            await unitOfWork.CompleteAsync();

            var companyExamViewModel = new CompanyExamViewModel(companyExam);
            return ApiResponseHelper.Success(companyExamViewModel, "Created successfully");
        }


        //Update an existing exam

        [HttpPut("{id:int}")]
        public async Task<ApiResponse<CompanyExamViewModel>> UpdateCompanyExam(int id, EditCompanyExamDTO editCompanyExamDTO)
        {
            if (editCompanyExamDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyExamViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var companyExam = await unitOfWork.CompanyExamRepository.GetByIdAsync(id);
            if (companyExam == null)
            {
                return ApiResponseHelper.Error<CompanyExamViewModel>(ErrorCode.NotFound, "Exam not found");
            }

            editCompanyExamDTO.UpdateEntity(companyExam);

            unitOfWork.CompanyExamRepository.Update(companyExam);
            await unitOfWork.CompleteAsync();


            var examViewModel = new CompanyExamViewModel(companyExam);
            return ApiResponseHelper.Success(examViewModel, "Exam Updated successfully");
        }

        //Soft delete an exam (set DeletedAt)

        [HttpDelete("{id:int}")]
        public async Task<ApiResponse<string>> SoftDeleteCompanyExam(int id)
        {
            var companyExamDeleted = await unitOfWork.CompanyExamRepository.SoftDeleteAsync(id);
            if (!companyExamDeleted)
            {
                return ApiResponseHelper.Error<string>(ErrorCode.NotFound, "Exam not found or already deleted.");
            }

            await unitOfWork.CompleteAsync();
            return ApiResponseHelper.Success("Exam deleted successfully.");
        }

        //Activate/deactivate an exam

        [HttpPost("{id:int}/activate")]
        public async Task<ApiResponse<bool>>ToggleActiveCompanyExamById(int id)
        {
            var companyExam = await unitOfWork.CompanyExamRepository.GetByIdAsync(id);
            if (companyExam == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "Exam not found");
            }
            if (companyExam.DeletedAt != null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.BadRequest, "Cannot activate a deleted exam.");
            }

            companyExam.IsActive = !companyExam.IsActive;
            companyExam.UpdatedAt = DateTime.UtcNow;

           unitOfWork.CompanyExamRepository.Update(companyExam);
            await unitOfWork.CompleteAsync();

            var message=companyExam.IsActive?"Exam activated successfully" : "Exam deactivated successfully";
            return ApiResponseHelper.Success(companyExam.IsActive, message);

        }


    }
}
