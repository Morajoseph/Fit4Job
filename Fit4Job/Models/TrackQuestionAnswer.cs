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

        [Required]
        [Display(Name = "Attempt ID")]
        public int AttemptId { get; set; }

        [Required]
        [Display(Name = "Question ID")]
        public int QuestionId { get; set; }

        [Column(TypeName = "json")]
        [Display(Name = "Selected Options")]
        public string? SelectedOptionsJson { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "Text Answer")]
        public string? TextAnswer { get; set; }

        [Required]
        [Display(Name = "Is Correct")]
        public bool IsCorrect { get; set; } = false;

        [Required]
        [Range(0, 999.99)]
        [Display(Name = "Points Earned")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PointsEarned { get; set; } = 0.00m;

        [Required]
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
        public string AnswerType => !string.IsNullOrEmpty(TextAnswer) ? "Text" :
                                   SelectedOptions.Any() ? "Multiple Choice" : "No Answer";

        // Navigation properties
        [Display(Name = "Attempt")]
        [ForeignKey("AttemptId")]
        public virtual TrackAttempt Attempt { get; set; } = null!;

        [Display(Name = "Question")]
        [ForeignKey("QuestionId")]
        public virtual TrackQuestion Question { get; set; } = null!;
    }
}
