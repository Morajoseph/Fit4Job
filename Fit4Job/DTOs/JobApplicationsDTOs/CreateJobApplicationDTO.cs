namespace Fit4Job.DTOs.JobApplicationsDTOs
{
    public class CreateJobApplicationDTO
    {
        [Display(Name = "User ID")]
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Display(Name = "Job ID")]
        [Required(ErrorMessage = "Job ID is required")]
        public int JobId { get; set; }

        public JobApplication ToEntity()
        {
            return new JobApplication()
            {
                JobId = this.JobId,
                UserId = this.UserId,
                Status = JobApplicationStatus.Incomplete
            };
        }
    }
}
