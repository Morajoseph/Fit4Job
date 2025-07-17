using Fit4Job.DTOs.CompanyExamQuestionAnswersDTOs;
using Fit4Job.ViewModels.CompanyExamQuestionAnswersViewModels;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyExamQuestionAnswersController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyExamQuestionAnswersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        /* ****************************************** Endpoints ****************************************** */

        [HttpGet("attempt/{attemptId:int}")]
        public async Task<ApiResponse<IEnumerable<CompanyExamQuestionAnswerViewModel>>> GetAnswerForAttempt(int attemptId)
        {
            var attempt = await _unitOfWork.CompanyExamAttemptRepository.GetByIdAsync(attemptId);
            if (attempt == null)
            {
                return ApiResponseHelper.Error<IEnumerable<CompanyExamQuestionAnswerViewModel>>(ErrorCode.NotFound, "Attempt Not Found");
            }

            var answers = await _unitOfWork.CompanyExamQuestionAnswerRepository.GetAllAsync();
            answers = answers.Where(a => a.AttemptId == attemptId).ToList();
            var data = answers.Select(a => CompanyExamQuestionAnswerViewModel.GetViewModel(a));
            return ApiResponseHelper.Success(data);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResponse<CompanyExamQuestionAnswerViewModel>> GetById(int id)
        {
            var answer = await _unitOfWork.CompanyExamQuestionAnswerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return ApiResponseHelper.Error<CompanyExamQuestionAnswerViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            return ApiResponseHelper.Success(CompanyExamQuestionAnswerViewModel.GetViewModel(answer));
        }

        [HttpGet("attempt/{attemptId:int}/question/{questionId:int}")]
        public async Task<ApiResponse<CompanyExamQuestionAnswerViewModel>> GetAnswerForQuestionInAttempt(int attemptId, int questionId)
        {
            var answer = await _unitOfWork.CompanyExamQuestionAnswerRepository.GetAnswerByAttemptAndQuestionAsync(attemptId, questionId);
            if (answer == null)
            {
                return ApiResponseHelper.Error<CompanyExamQuestionAnswerViewModel>(ErrorCode.NotFound, "Not Found or invalid IDs");
            }
            return ApiResponseHelper.Success(CompanyExamQuestionAnswerViewModel.GetViewModel(answer));
        }

        [HttpPost("submit")]
        public async Task<ApiResponse<CompanyExamQuestionAnswerViewModel>> Create(CreateCompanyExamQuestionAnswerDTO createAnswerDTO)
        {
            // Validation 
            if (createAnswerDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<CompanyExamQuestionAnswerViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var attempt = await _unitOfWork.CompanyExamAttemptRepository.GetByIdAsync(createAnswerDTO.AttemptId);
            if (attempt == null)
            {
                return ApiResponseHelper.Error<CompanyExamQuestionAnswerViewModel>(ErrorCode.BadRequest, "Not Found or invalid attempt ID");
            }

            var question = await _unitOfWork.CompanyExamQuestionRepository.GetByIdAsync(createAnswerDTO.QuestionId);
            if (question == null)
            {
                return ApiResponseHelper.Error<CompanyExamQuestionAnswerViewModel>(ErrorCode.BadRequest, "Not Found or invalid question ID");
            }

            var questionAnswer = await ProcessQuestionAnswerAsync(question, attempt, createAnswerDTO);

            await _unitOfWork.CompanyExamQuestionAnswerRepository.AddAsync(questionAnswer);
            await _unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(CompanyExamQuestionAnswerViewModel.GetViewModel(questionAnswer), "Answer Submited successfully");
        }

        /* *************************************** Helper Methods **************************************** */

        private async Task<CompanyExamQuestionAnswer> ProcessQuestionAnswerAsync(CompanyExamQuestion question, CompanyExamAttempt attempt, CreateCompanyExamQuestionAnswerDTO answerDTO)
        {
            bool isCorrect;
            decimal pointsEarned = 0.00m;
            if (question.QuestionType == QuestionType.TrueFalse || question.QuestionType == QuestionType.MultipleChoiceSingle) // for  Single Choice Questions
            {
                isCorrect = await ProcessSingleChoiceQuestionAsync(question.Id, answerDTO);
            }
            else if (question.QuestionType == QuestionType.MultipleChoiceMultiple)  // For Multiple Choice Questions
            {
                isCorrect = await ProcessMultipleChoiceQuestionsAsync(question.Id, answerDTO);
            }
            else  // For Written Questions
            {
                isCorrect = await ProcessWrittenQuestionsAsync(question.Id, answerDTO);
            }
            pointsEarned = isCorrect ? question.Points : 0.00m;

            //var countQuestionsInTrack = await _unitOfWork.CompanyExamQuestionRepository.CountQuestionsInTrackAsync(attempt.TrackId);
            attempt.Score += pointsEarned;
            //attempt.SolvedQuestionsCount++;
            var exam = await _unitOfWork.CompanyExamRepository.GetByIdAsync(attempt.ExamId);
            if (exam == null)
            {
                throw new Exception("Exam not found for the given attempt.");
            }
            attempt.PercentageScore = (attempt.Score / exam.TotalScore) * 100;

            var companyExamQuestionAnswer = answerDTO.ToEntity();
            companyExamQuestionAnswer.IsCorrect = isCorrect;
            companyExamQuestionAnswer.PointsEarned = pointsEarned;

            return companyExamQuestionAnswer;
        }

        private static bool IsValidSingleChoiceAnswer(CreateCompanyExamQuestionAnswerDTO answerDTO)
        {
            return answerDTO?.SelectedOptions != null &&
                   answerDTO.SelectedOptions.Count == 1 &&
                   answerDTO.SelectedOptions[0] > 0;
        }

        private async Task<bool> ProcessSingleChoiceQuestionAsync(int questionId, CreateCompanyExamQuestionAnswerDTO answerDTO)
        {
            if (!IsValidSingleChoiceAnswer(answerDTO))
            {
                return false;
            }

            var selectedOptionId = answerDTO.SelectedOptions[0];
            var correctOptions = await _unitOfWork.CompanyExamQuestionOptionRepository.GetCorrectOptionsByQuestionIdAsync(questionId);
            if (correctOptions == null || !correctOptions.Any())
            {
                return false;
            }

            var isCorrect = correctOptions.Any(option => option.Id == selectedOptionId);
            return isCorrect;
        }

        private async Task<bool> ProcessMultipleChoiceQuestionsAsync(int questionId, CreateCompanyExamQuestionAnswerDTO answerDTO)
        {
            var correctOptions = await _unitOfWork.CompanyExamQuestionOptionRepository.GetCorrectOptionsByQuestionIdAsync(questionId);
            if (correctOptions.Count() == 0)
            {
                return answerDTO.SelectedOptions == null || answerDTO.SelectedOptions.Count() == 0;
            }
            if (answerDTO.SelectedOptions == null || answerDTO.SelectedOptions.Count != correctOptions.Count())
            {
                return false;
            }
            foreach (var optionId in answerDTO.SelectedOptions)
            {
                var isCorrect = correctOptions.Any(option => option.Id == optionId);
                if (!isCorrect) return false;
            }
            return true;
        }

        private async Task<bool> ProcessWrittenQuestionsAsync(int questionId, CreateCompanyExamQuestionAnswerDTO answerDTO)
        {
            var correctOptions = await _unitOfWork.CompanyExamQuestionOptionRepository.GetCorrectOptionsByQuestionIdAsync(questionId);
            if (correctOptions.Count() == 0)
            {
                return true;
            }
            if (answerDTO.TextAnswer == null)
            {
                return false;
            }
            var isCorrect = correctOptions.Any(option => option.OptionText.Contains(answerDTO.TextAnswer));
            return isCorrect;
        }
    }
}
