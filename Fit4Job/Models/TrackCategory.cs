namespace Fit4Job.Models
{
    [Table("track_categories")]
    [Index(nameof(Name), IsUnique = true, Name = "IX_TrackCategories_Name")]
    public class TrackCategory
    {
        [Key]
        [Display(Name = "Track Category ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "Track Name")]
        public string Name { get; set; } = string.Empty;



        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        [Column(TypeName = "text")]
        [Display(Name = "Category Description")]
      
        public string? Description { get; set; }


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


        // Computed properties
        [NotMapped]
        public bool IsActive => DeletedAt == null;

        // Navigation properties
        public virtual ICollection<Track>? Tracks { get; set; }
    }
}