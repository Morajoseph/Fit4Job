using Microsoft.AspNetCore.Http.HttpResults;

namespace Fit4Job.Models
{

    [Table("Tracks")]
    [Index(nameof(CategoryId))]
    [Index(nameof(IsPremium))]
    [Index(nameof(IsActive))]
    [Index(nameof(CreatedBy))]

    public class Track
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Track Name")]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "Track Description")]
        public string? Description { get; set; }

        public bool IsPremium { get; set; }=false;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Price { get; set; } = 0;


        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Track Total Score")]
        public decimal TrackTotalScore { get; set; }



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


        // Navigation property
        [ForeignKey("UserId")]
        public virtual ApplicationUser CreatedBy { get; set; } = null!;


        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public PracticeCategory Category { get; set; }

        // Helper properties
        [NotMapped]
        public bool IsActive => DeletedAt == null;

    }
}
