namespace Fit4Job.Models
{
    [Table("payments")]
    [Index(nameof(UserId), Name = "IX_Payment_UserId")]
    [Index(nameof(PaymentStatus), Name = "IX_Payment_PaymentStatus")]
    [Index(nameof(TransactionId), IsUnique = true, Name = "IX_Payment_TransactionId")]
    [Index(nameof(CreatedAt), Name = "IX_Payment_CreatedAt")]
    [Index(nameof(PaymentMethod), Name = "IX_Payment_PaymentMethod")]
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public int UserId { get; set; }


        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }


        [StringLength(3)]
        [Display(Name = "Currency")]
        public string Currency { get; set; } = "USD";


        [Required]
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }


        [Required]
        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.pending;

        [StringLength(255)]
        [Display(Name = "Transaction ID")]
        public string? TransactionId { get; set; }


        [StringLength(255)]
        [Display(Name = "Payment Token")]
        public string? PaymentToken { get; set; }


        [Column(TypeName = "json")]
        [Display(Name = "Payment Gateway Response")]
        public string? PaymentGatewayResponse { get; set; }


        [Column(TypeName = "text")]
        [Display(Name = "Refund Reason")]
        public string? RefundReason { get; set; }


        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Refunded Amount")]
        public decimal RefundedAmount { get; set; } = 0;


        [DataType(DataType.DateTime)]
        [Display(Name = "Refunded At")]
        public DateTime? RefundedAt { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Helper property
        [NotMapped]
        public bool IsRefunded => RefundedAt != null && RefundedAmount > 0;


        public PaymentMethod Payment_Method { get; set; }


        public PaymentStatus paymentStatus { get; set; } = PaymentStatus.pending;

        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

    }
}
