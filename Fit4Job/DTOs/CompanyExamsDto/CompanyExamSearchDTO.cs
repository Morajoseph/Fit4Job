namespace Fit4Job.DTOs.CompanyExamsDto
{
    public class CompanyExamSearchDTO
    {
        public int? CompanyId { get; set; }
        public int? JobId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    }
