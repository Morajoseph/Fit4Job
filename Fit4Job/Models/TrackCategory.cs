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
        [StringLength(100)]
        [Display(Name = "Category Name")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Url(ErrorMessage = "Please enter a valid URL")]
        [StringLength(500, ErrorMessage = "Icon URL cannot exceed 500 characters")]
        [Column(TypeName = "varchar(500)")]
        [Display(Name = "Icon URL")]
        public string? IconUrl { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        [Column(TypeName = "nvarchar(1000)")]
        [Display(Name = "Category Description")]
        public string? Description { get; set; }

        [Display(Name = "Category Skills")]
        [Column(TypeName = "nvarchar(max)")]
        public string? Skills { get; set; } = string.Empty;

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

        [NotMapped]
        [Display(Name = "Category Skills")]
        public List<int> CategorySkills
        {
            get
            {
                if (string.IsNullOrEmpty(Skills))
                    return new List<int>();

                try
                {
                    return JsonSerializer.Deserialize<List<int>>(Skills) ?? new List<int>();
                }
                catch
                {
                    return new List<int>();
                }
            }
            set
            {
                Skills = value?.Count > 0 ? JsonSerializer.Serialize(value) : null;
            }
        }

        // Navigation properties
        public virtual ICollection<Track>? Tracks { get; set; }
    }
}