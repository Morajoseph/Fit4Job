namespace Fit4Job.DTOs.CompanyExamsDto
{
    public class EditCompanyExamDTO
    {
        [StringLength(256, MinimumLength = 5)]
        [Display(Name = "Title")]
        public string? Title { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [StringLength(2000)]
        [Display(Name = "Instructions")]
        public string? Instructions { get; set; }

        [Range(1, 600)]
        [Display(Name = "Duration (Minutes)")]
        public int? DurationMinutes { get; set; }

        [Range(0, 99999999.99)]
        [Display(Name = "Passing Score")]
        public decimal? PassingScore { get; set; }

        [Display(Name = "Show Results Immediately")]
        public bool? ShowResultsImmediately { get; set; }

        public EditCompanyExamDTO()
        {
        }

        public EditCompanyExamDTO(CompanyExam exam)
        {
            Title = exam.Title;
            Description = exam.Description;
            Instructions = exam.Instructions;
            DurationMinutes = exam.DurationMinutes;
            PassingScore = exam.PassingScore;
            ShowResultsImmediately = exam.ShowResultsImmediately;
        }

        public void UpdateEntity(CompanyExam exam)
        {
            if (!string.IsNullOrWhiteSpace(Title))
                exam.Title = Title;

            if (!string.IsNullOrWhiteSpace(Description))
                exam.Description = Description;

            if (!string.IsNullOrWhiteSpace(Instructions))
                exam.Instructions = Instructions;

            if (DurationMinutes.HasValue)
                exam.DurationMinutes = DurationMinutes.Value;

            
            if (PassingScore.HasValue)
                exam.PassingScore = PassingScore.Value;

            if (ShowResultsImmediately.HasValue)
                exam.ShowResultsImmediately = ShowResultsImmediately.Value;

            exam.UpdatedAt = DateTime.UtcNow;
        }
    }
}
