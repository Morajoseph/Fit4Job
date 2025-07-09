namespace Fit4Job.Models
{
    [Table("track_question_options")]
    [Index(nameof(QuestionId), Name = "IX_TrackQuestionOptions_QuestionId")]
    [Index(nameof(IsCorrect), Name = "IX_TrackQuestionOptions_IsCorrect")]
    public class TrackQuestionOption
    {
        [Key]
        [Display(Name = "Option ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Question ID")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Option text is required")]
        [Column(TypeName = "nvarchar(2000)")]
        [Display(Name = "Option Text")]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Option text must be between 1 and 2000 characters")]
        public string OptionText { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Is Correct")]
        public bool IsCorrect { get; set; } = false;

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Display(Name = "Deleted At")]
        public DateTime? DeletedAt { get; set; }

        // Computed property
        [NotMapped]
        public bool IsActive => DeletedAt == null;

        // Navigation property
        [Display(Name = "Question")]
        [ForeignKey("QuestionId")]
        public virtual TrackQuestion Question { get; set; } = null!;
    }
}
