namespace Fit4Job.DTOs.CompanyExamAttemptsDTOs
{
    public class CreateCompanyExamAttemptDTO
    {
        [Display(Name = "User ID")]
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Display(Name = "Exam ID")]
        [Required(ErrorMessage = "Exam ID is required")]
        public int ExamId { get; set; }

        [Display(Name = "Job Application ID")]
        [Required(ErrorMessage = "Job Application ID is required")]
        public int JobApplicationId { get; set; }

        public CompanyExamAttempt ToEntity()
        {
            return new CompanyExamAttempt()
            {
                UserId = this.UserId,
                ExamId = this.ExamId,
                JobApplicationId = this.JobApplicationId,
                StartTime = DateTime.UtcNow, // Default to current time
                EndTime = null, // Initially null
                Score = 0.00m, // Default score
                PercentageScore = 0.00m, // Default percentage score
                Status = CompanyExamAttemptStatus.InProgress, // Default status
                Passed = false, // Default passed status
                CreatedAt = DateTime.UtcNow, // Default created time
                UpdatedAt = DateTime.UtcNow // Default updated time
            };
        }
    }
}
