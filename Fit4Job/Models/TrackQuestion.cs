namespace Fit4Job.Models
{
    [Table("track_questions")]
    [Index(nameof(TrackId), Name = "IX_TrackQuestions_TrackId")]
    [Index(nameof(IsActive), Name = "IX_TrackQuestions_IsActive")]
    [Index(nameof(QuestionType), Name = "IX_TrackQuestions_QuestionType")]
    [Index(nameof(QuestionLevel), Name = "IX_TrackQuestions_QuestionLevel")]
    [Index(nameof(DifficultyLevel), Name = "IX_TrackQuestions_DifficultyLevel")]
    public class TrackQuestion
    {
        [Key]
        [Display(Name = "Question ID")]
        public int Id { get; set; }


        [Required]
        [Display(Name = "Track ID")]
        public int TrackId { get; set; }


        [Required]
        [Column(TypeName = "text")]
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; }


        [Required]
        [Display(Name = "Question Type")]
        [Column(TypeName = "varchar(30)")]
        [EnumDataType(typeof(QuestionType))]
        public QuestionType QuestionType { get; set; }


        [Required]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Question Level")]
        [EnumDataType(typeof(QuestionLevel))]
        public QuestionLevel QuestionLevel { get; set; }


        [Required]
        [Column(TypeName = "varchar(10)")]
        [Display(Name = "Difficulty Level")]
        [EnumDataType(typeof(DifficultyLevel))]
        public DifficultyLevel DifficultyLevel { get; set; }


        [Required]
        [Range(0, 999.99)]
        [Display(Name = "Points")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Points { get; set; }


        [Column(TypeName = "text")]
        [Display(Name = "Explanation")]
        public string? Explanation { get; set; }


        [Column(TypeName = "text")]
        [Display(Name = "Code Snippet")]
        public string? CodeSnippet { get; set; }


        [Column(TypeName = "text")]
        [Display(Name = "Expected Output")]
        public string? ExpectedOutput { get; set; }


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
        [ForeignKey("TrackId")]
        [Display(Name = "Track")]
        public virtual Track Track { get; set; } = null!;

        public virtual ICollection<TrackQuestionOption>? Options { get; set; }
        public virtual ICollection<TrackQuestionAnswer>? Answers { get; set; }

    }
}
