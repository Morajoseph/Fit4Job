using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fit4Job.Models
{
    [Table("track_questions")]
    [Index(nameof(TrackId))]
    [Index(nameof(QuestionType))]
    [Index(nameof(DifficultyLevel))]
    [Index(nameof(QuestionLevel))]
    [Index(nameof(IsActive))]
    public class TrackQuestion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Track ID")]
        [ForeignKey("Track")]
        public int TrackId { get; set; }
        public Track Track { get; set; }

        [Required]
        [Column(TypeName = "text")]
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; }

        [Required]
        [Display(Name = "Question Type")]
        public QuestionType QuestionType { get; set; }

        [Required]
        [Display(Name = "Question Level")]
        public QuestionLevel QuestionLevel { get; set; }

        [Required]
        [Display(Name = "Difficulty Level")]
        public DifficultyLevel DifficultyLevel { get; set; }

        [Required]
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

        // Helper property
        [NotMapped]
        public bool IsActive => DeletedAt == null ;
    }

  
}
