namespace Fit4Job.Models
{
    [Table("company_exam_question_options")]
    [Index(nameof(QuestionId), Name = "IX_CompanyExamQuestionOptions_QuestionId")]
    [Index(nameof(IsCorrect), Name = "IX_CompanyExamQuestionOptions_IsCorrect")]
    [Index(nameof(IsActive), Name = "IX_CompanyExamQuestionOptions_IsActive")]
    public class CompanyExamQuestionOption
    {
        [Key]
        [Display(Name = "Option ID")]
        public int Id { get; set; }


        [Required(ErrorMessage = "Question ID is required")]
        [Display(Name = "Question ID", Description = "Reference to the parent question")]
        [Range(1, int.MaxValue, ErrorMessage = "Question ID must be a positive number")]
        public int QuestionId { get; set; }


        [Required(ErrorMessage = "Option text is required")]
        [Column(TypeName = "nvarchar(2000)")]
        [Display(Name = "Option Text", Description = "The text content of this answer option")]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Option text must be between 1 and 2,000 characters")]
        [DataType(DataType.MultilineText)]
        public string OptionText { get; set; } = string.Empty;


        [Required(ErrorMessage = "Is Correct is required")]
        [Display(Name = "Is Correct", Description = "Whether this option is the correct answer")]
        public bool IsCorrect { get; set; } = false;


        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }

        // Computed properties
        [NotMapped]
        public bool IsActive => DeletedAt == null;

        // Navigation property
        [ForeignKey("QuestionId")]
        public virtual CompanyExamQuestion Question { get; set; } = null!;
    }
}
