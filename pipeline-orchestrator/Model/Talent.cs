using pipeline_orchestrator.Model.Recruit;
using System.ComponentModel.DataAnnotations;

namespace pipeline_orchestrator.Model
{
    public class Talent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(150)]
        [Required]
        public string Name { get; set; } = string.Empty;
        [MaxLength(150)]
        [Required(ErrorMessage = "Email message is required")]
        [EmailAddress(ErrorMessage = "Illegal format email")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(8000)]
        public string? Experience { get; set; } = string.Empty; // Basically we send this to microservice which will feed this to LLM
        [MaxLength(8000)]
        public string? Projects { get; set; } = string.Empty;
        [MaxLength(5000)]
        public string? ProfessionalSummary { get; set; } = string.Empty;
        [MaxLength(8000)]
        public string? TechnicalSkills { get; set; } = string.Empty;

    }
}
