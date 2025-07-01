namespace Fit4Job.Models
{
    [Table("track_questions")]
    [Index(nameof(TrackId), Name = "IX_TrackQuestions_TrackId")]
    [Index(nameof(QuestionType), Name = "IX_TrackQuestions_QuestionType")]
    public class TrackQuestion
    {
        [Key]
        [Display(Name = "Question ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Track is required")]
        [Display(Name = "Track ID")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid track")]

        public int TrackId { get; set; }


        [Required(ErrorMessage = "Question text is required")]
        [StringLength(5000, MinimumLength = 5, ErrorMessage = "Question text must be between 5 and 5000 characters")]
        [Column(TypeName = "text")]
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; }


        [Required(ErrorMessage = "Question level is required")]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Question Level")]
        [EnumDataType(typeof(QuestionType))]
        public QuestionType QuestionType { get; set; }


        [Required]
        [Column(TypeName = "varchar(20)")]
        [Display(Name = "Question Level")]
        [EnumDataType(typeof(QuestionLevel))]
        public QuestionLevel QuestionLevel { get; set; }


        [Required(ErrorMessage = "Difficulty level is required")]
        [Column(TypeName = "varchar(10)")]
        [Display(Name = "Difficulty Level")]
        [EnumDataType(typeof(DifficultyLevel), ErrorMessage = "Please select a valid difficulty level")]
        public DifficultyLevel DifficultyLevel { get; set; }


        [Required(ErrorMessage = "Points value is required")]
        [Range(0.01, 999.99, ErrorMessage = "Points must be between 0.01 and 999.99")]
        [Display(Name = "Points")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Points { get; set; }

        [StringLength(2000, ErrorMessage = "Explanation cannot exceed 2000 characters")]
        [Column(TypeName = "text")]
        [Display(Name = "Explanation")]

        public string? Explanation { get; set; }


        [StringLength(10000, ErrorMessage = "Code snippet cannot exceed 10000 characters")]
        [Column(TypeName = "text")]
        [Display(Name = "Code Snippet")]
        public string? CodeSnippet { get; set; }


        [StringLength(2000, ErrorMessage = "Expected output cannot exceed 2000 characters")]
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
        [Display(Name = "Track")]
        [ForeignKey("TrackId")]
        public virtual Track Track { get; set; } = null!;
        public virtual ICollection<TrackQuestionOption>? Options { get; set; }
        public virtual ICollection<TrackQuestionAnswer>? Answers { get; set; }

    }
}
