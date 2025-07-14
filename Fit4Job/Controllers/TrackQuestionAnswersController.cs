using Fit4Job.DTOs.TrackQuestionAnswersDTOs;
using Fit4Job.ViewModels.TrackQuestionAnswersViewModels;

namespace Fit4Job.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackQuestionAnswersController : ControllerBase
    {
        /* ********************************************* DI ********************************************** */
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public TrackQuestionAnswersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        /* ****************************************** Endpoints ****************************************** */

        // 1 - Get a specific track question answer by ID.
        [HttpGet("{id:int}")]
        public async Task<ApiResponse<TrackQuestionAnswerViewModel>> GetById(int id)
        {
            var answer = await unitOfWork.TrackQuestionAnswerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return ApiResponseHelper.Error<TrackQuestionAnswerViewModel>(ErrorCode.NotFound, "Not Found or invalid ID");
            }
            return ApiResponseHelper.Success(new TrackQuestionAnswerViewModel(answer));
        }

        // 2 - Get all answers for a specific attempt
        [HttpGet("by-attempt/{id:int}")]
        public async Task<ApiResponse<IEnumerable<TrackQuestionAnswerViewModel>>> GetByAttemptId(int id)
        {
            var attempt = await unitOfWork.TrackAttemptRepository.GetByIdAsync(id);
            if (attempt == null)
            {
                return ApiResponseHelper.Error<IEnumerable<TrackQuestionAnswerViewModel>>(ErrorCode.NotFound, "Attempt Not Found");
            }

            var answers = await unitOfWork.TrackQuestionAnswerRepository.GetAllAnswersByAttemptAsync(id);
            var data = answers.Select(a => TrackQuestionAnswerViewModel.GetViewModel(a));
            return ApiResponseHelper.Success(data);
        }

        // 3 - Get the answer for a specific question within an attempt
        [HttpGet("attempt/{attemptId:int}/question/{questionId:int}")]
        public async Task<ApiResponse<TrackQuestionAnswerViewModel>> GetAnswerForQuestionInAttempt(int attemptId, int questionId)
        {
            var answer = await unitOfWork.TrackQuestionAnswerRepository.GetAnswerForQuestionInAttemptAsync(attemptId, questionId);
            if (answer == null)
            {
                return ApiResponseHelper.Error<TrackQuestionAnswerViewModel>(ErrorCode.NotFound, "Not Found or invalid IDs");
            }
            var data = TrackQuestionAnswerViewModel.GetViewModel(answer);
            return ApiResponseHelper.Success(data);
        }

        // 4 - Create a new track question answer 
        [HttpPost("submit")]
        public async Task<ApiResponse<TrackQuestionAnswerViewModel>> Create(TrackQuestionAnswerDTO createAnswerDTO)
        {
            // Validation 
            if (createAnswerDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackQuestionAnswerViewModel>(ErrorCode.BadRequest, "Invalid data");
            }

            var attempt = await unitOfWork.TrackAttemptRepository.GetByIdAsync(createAnswerDTO.AttemptId);
            if (attempt == null)
            {
                return ApiResponseHelper.Error<TrackQuestionAnswerViewModel>(ErrorCode.BadRequest, "Not Found or invalid attempt ID");
            }

            var question = await unitOfWork.TrackQuestionRepository.GetByIdAsync(createAnswerDTO.QuestionId);
            if (question == null)
            {
                return ApiResponseHelper.Error<TrackQuestionAnswerViewModel>(ErrorCode.BadRequest, "Not Found or invalid question ID");
            }

            var questionAnswer = await ProcessQuestionAnswerAsync(question, attempt, createAnswerDTO);

            await unitOfWork.TrackQuestionAnswerRepository.AddAsync(questionAnswer);
            await unitOfWork.CompleteAsync();
            var answerViewModel = new TrackQuestionAnswerViewModel(questionAnswer);

            return ApiResponseHelper.Success(answerViewModel, "Answer Submited successfully");
        }

        // 5 - Update an existing track question answer.
        [HttpPut("{id:int}")]
        public async Task<ApiResponse<TrackQuestionAnswerViewModel>> Update(int id, EditTrackQuestionAnswerDTO editQuestionAnswerDTO)
        {
            // Still Need to complet it's logic.

            if (editQuestionAnswerDTO == null || !ModelState.IsValid)
            {
                return ApiResponseHelper.Error<TrackQuestionAnswerViewModel>(ErrorCode.BadRequest, "Invalid data");
            }
            var questionAnswer = await unitOfWork.TrackQuestionAnswerRepository.GetByIdAsync(id);
            if(questionAnswer == null)
            {
                return ApiResponseHelper.Error<TrackQuestionAnswerViewModel>(ErrorCode.NotFound, "Question Answer not found");
            }
            editQuestionAnswerDTO.UpdateEntity(questionAnswer);
            unitOfWork.TrackQuestionAnswerRepository.Update(questionAnswer);
            await unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(TrackQuestionAnswerViewModel.GetViewModel(questionAnswer), "Updated successfully");

        }

        [HttpDelete("{id:int}")]
        public async Task<ApiResponse<bool>> Delete(int id)
        {
            // Still Need to complet it's logic.

            var questionAnswer = await unitOfWork.TrackQuestionAnswerRepository.GetByIdAsync(id);
            if(questionAnswer == null)
            {
                return ApiResponseHelper.Error<bool>(ErrorCode.NotFound, "Question Answer not found");
            }
            unitOfWork.TrackQuestionAnswerRepository.Delete(questionAnswer);
            await unitOfWork.CompleteAsync();

            return ApiResponseHelper.Success(true, "Deleted successfully");
        }

        /* *************************************** Helper Methods **************************************** */

        private async Task<TrackQuestionAnswer> ProcessQuestionAnswerAsync(TrackQuestion question, TrackAttempt attempt, TrackQuestionAnswerDTO answerDTO)
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

            var countQuestionsInTrack = await unitOfWork.TrackQuestionRepository.CountQuestionsInTrackAsync(attempt.TrackId);
            attempt.TotalScore += pointsEarned;
            attempt.SolvedQuestionsCount++;
            attempt.ProgressPercentage = (int)(((decimal)attempt.SolvedQuestionsCount / (decimal)countQuestionsInTrack) * 100);
            if(attempt.ProgressPercentage == 100)
            {
                attempt.Status = AttemptStatus.Completed;
            }

            var trackQuestionAnswer = answerDTO.GetModel();
            trackQuestionAnswer.IsCorrect = isCorrect;
            trackQuestionAnswer.PointsEarned = pointsEarned;

            return trackQuestionAnswer;
        }

        private static bool IsValidSingleChoiceAnswer(TrackQuestionAnswerDTO answerDTO)
        {
            return answerDTO?.SelectedOptions != null &&
                   answerDTO.SelectedOptions.Count == 1 &&
                   answerDTO.SelectedOptions[0] > 0;
        }

        private async Task<bool> ProcessSingleChoiceQuestionAsync(int questionId, TrackQuestionAnswerDTO answerDTO)
        {
            if (!IsValidSingleChoiceAnswer(answerDTO))
            {
                return false;
            }

            var selectedOptionId = answerDTO.SelectedOptions[0];
            var correctOptions = await unitOfWork.TrackQuestionOptionRepository.GetCorrectOptionsAsync(questionId);
            if (correctOptions == null || !correctOptions.Any())
            {
                return false;
            }

            var isCorrect = correctOptions.Any(option => option.Id == selectedOptionId);
            return isCorrect;
        }

        private async Task<bool> ProcessMultipleChoiceQuestionsAsync(int questionId, TrackQuestionAnswerDTO answerDTO)
        {
            var correctOptions = await unitOfWork.TrackQuestionOptionRepository.GetCorrectOptionsAsync(questionId);
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

        private async Task<bool> ProcessWrittenQuestionsAsync(int questionId, TrackQuestionAnswerDTO answerDTO)
        {
            var correctOptions = await unitOfWork.TrackQuestionOptionRepository.GetCorrectOptionsAsync(questionId);
            if(correctOptions.Count() == 0)
            {
                return true;
            }
            if(answerDTO.TextAnswer == null )
            {
                return false;
            }    
            var isCorrect = correctOptions.Any(option => option.OptionText.Contains(answerDTO.TextAnswer));
            return isCorrect;
        }
    }
}
