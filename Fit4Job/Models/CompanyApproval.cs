using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fit4Job.Models
{
    public class CompanyApproval
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public CompanyProfile Company {  get; set; }



        [ForeignKey("ApprovedByUser")]
        public int ApprovedBy { get; set; }
        public ApplicationUser ApprovedByUser { get; set; }

        public enum ApprovalStatus
        {
            pending,
            approved,
            rejected
        }

        [Required]
        public ApprovalStatus PreviousStatus { get; set; }

        [Required]
        public ApprovalStatus NewStatus { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ApprovalDate { get; set; } = DateTime.Now;

        public string? RejectionReason { get; set; }

        public string? AdminNotes { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


    }
}
