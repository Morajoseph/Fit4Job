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


        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public int UserId { get; set; }


        [Required(ErrorMessage = "Amount is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 999999999.99, ErrorMessage = "Amount must be between 0.01 and 999,999,999.99")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }


        [Required(ErrorMessage = "Currency is required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be exactly 3 characters")]
        [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency must be a valid 3-letter uppercase code (e.g., USD, EUR)")]
        [Display(Name = "Currency")]
        [Column(TypeName = "varchar(3)")]
        public string Currency { get; set; } = "USD";


        [Required(ErrorMessage = "Payment method is required")]
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }


        [Required(ErrorMessage = "Payment status is required")]
        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.pending;


        [StringLength(255, ErrorMessage = "Transaction ID cannot exceed 255 characters")]
        [Display(Name = "Transaction ID")]
        [Column(TypeName = "varchar(255)")]
        public string? TransactionId { get; set; }


        [StringLength(500, ErrorMessage = "Payment token cannot exceed 500 characters")]
        [Display(Name = "Payment Token")]
        [Column(TypeName = "varchar(500)")]
        public string? PaymentToken { get; set; }


        [StringLength(4000, ErrorMessage = "Payment gateway response cannot exceed 4000 characters")]
        [Display(Name = "Payment Gateway Response")]
        [Column(TypeName = "varchar(max)")]
        public string? PaymentGatewayResponse { get; set; }


        [StringLength(1000, ErrorMessage = "Refund reason cannot exceed 1000 characters")]
        [Display(Name = "Refund Reason")]
        [Column(TypeName = "varchar(1000)")]
        public string? RefundReason { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 999999999.99, ErrorMessage = "Refunded amount must be between 0 and 999,999,999.99")]
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
        public DateTime UpdatedAt { get; set; }

        // Helper properties
        [NotMapped]
        [Display(Name = "Is Refunded")]
        public bool IsRefunded => RefundedAt.HasValue && RefundedAmount > 0;

        [NotMapped]
        [Display(Name = "Is Completed")]
        public bool IsCompleted => PaymentStatus == PaymentStatus.completed;

        [NotMapped]
        [Display(Name = "Is Pending")]
        public bool IsPending => PaymentStatus == PaymentStatus.pending;

        [NotMapped]
        [Display(Name = "Remaining Amount")]
        public decimal RemainingAmount => Amount - RefundedAmount;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
}