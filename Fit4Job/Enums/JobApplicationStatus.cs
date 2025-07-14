namespace Fit4Job.Enums
{
    public enum JobApplicationStatus
    {
        [Display(Name = "Incomplete", Description = "Application is not yet completed")]
        Incomplete = 0,

        [Display(Name = "Under Review", Description = "Application is being reviewed by HR")]
        UnderReview = 1,

        [Display(Name = "Interview", Description = "Candidate is in interview process")]
        Interview = 2,

        [Display(Name = "Accepted", Description = "Application has been accepted")]
        Accepted = 3,

        [Display(Name = "Rejected", Description = "Application has been rejected")]
        Rejected = 4,

        [Display(Name = "Withdrawn", Description = "Application has been withdrawn by candidate")]
        Withdrawn = 5
    }
}
