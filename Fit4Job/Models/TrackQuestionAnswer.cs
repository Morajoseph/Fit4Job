namespace Fit4Job.Models
{
    [Table("track_question_answers")]
    [Index(nameof(AttemptId), Name = "IX_TrackQuestionAnswers_AttemptId")]
    [Index(nameof(QuestionId), Name = "IX_TrackQuestionAnswers_QuestionId")]
    [Index(nameof(AttemptId), nameof(QuestionId), IsUnique = true, Name = "IX_TrackQuestionAnswers_AttemptId_QuestionId")]
    public class TrackQuestionAnswer
    {
        [Key]
        [Display(Name = "Answer ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Attempt ID is required")]
        [Display(Name = "Attempt ID")]
        public int AttemptId { get; set; }

        [Required(ErrorMessage = "Question ID is required")]
        [Display(Name = "Question ID")]
        public int QuestionId { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Selected Options")]
        public string? SelectedOptionsJson { get; set; }

        [Column(TypeName = "nvarchar(2000)")]
        [Display(Name = "Text Answer")]
        [MaxLength(2000, ErrorMessage = "Text answer cannot exceed 2,000 characters")]
        public string? TextAnswer { get; set; }

        [Required(ErrorMessage = "Is Correct is required")]
        [Display(Name = "Is Correct")]
        public bool IsCorrect { get; set; } = false;

        [Range(0, 9999.99)]
        [Display(Name = "Points Earned")]
        [Column(TypeName = "decimal(5,2)")]
        [Required(ErrorMessage = "Points Earned is required")]
        public decimal PointsEarned { get; set; } = 0.00m;

        [DataType(DataType.DateTime)]
        [Display(Name = "Answered At")]
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;

        // Computed properties for working with selected options
        [NotMapped]
        [Display(Name = "Selected Options")]
        public List<int> SelectedOptions
        {
            get
            {
                if (string.IsNullOrEmpty(SelectedOptionsJson))
                    return new List<int>();

                try
                {
                    return JsonSerializer.Deserialize<List<int>>(SelectedOptionsJson) ?? new List<int>();
                }
                catch
                {
                    return new List<int>();
                }
            }
            set
            {
                SelectedOptionsJson = value?.Count > 0 ? JsonSerializer.Serialize(value) : null;
            }
        }

        [NotMapped]
        [Display(Name = "Has Answer")]
        public bool HasAnswer => !string.IsNullOrEmpty(TextAnswer) || SelectedOptions.Any();

        [NotMapped]
        [Display(Name = "Answer Type")]
        public string AnswerType => !string.IsNullOrEmpty(TextAnswer) ? "Text" : SelectedOptions.Any() ? "Multiple Choice" : "No Answer";

        // Navigation properties
        [Display(Name = "Attempt")]
        [ForeignKey("AttemptId")]
        public virtual TrackAttempt Attempt { get; set; } = null!;

        [Display(Name = "Question")]
        [ForeignKey("QuestionId")]
        public virtual TrackQuestion Question { get; set; } = null!;
    }
}
