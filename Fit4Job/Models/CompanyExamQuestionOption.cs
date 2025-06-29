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


        [Required]
        [Display(Name = "Question ID")]
        public int QuestionId { get; set; }


        [Required]
        [Column(TypeName = "text")]
        [Display(Name = "Option Text")]
        public string OptionText { get; set; } = string.Empty;


        [Required]
        [Display(Name = "Is Correct")]
        public bool IsCorrect { get; set; } = false;


        [Required]
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
        [Display(Name = "Question")]
        public virtual CompanyExamQuestion Question { get; set; } = null!;
    }
}
