using System.ComponentModel.DataAnnotations;

namespace pipeline_orchestrator.Model
{
    public class Experience
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(3000)]
        public string RoleDescription { get; set; } = string.Empty;
        public bool currentlyWorking { get; set; } = false;
        [Required]

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }  =DateTime.UtcNow;

        [MaxLength(150)]
        [Required(ErrorMessage = "Institution name is required")]
        public string InstitutionName { get; set; } = string.Empty;

        [MaxLength(3000)]
        public string? GitHubUrl { get; set; } = string.Empty; // For someone who does not have a repo, we need to do data science on company experience 

    }
}
