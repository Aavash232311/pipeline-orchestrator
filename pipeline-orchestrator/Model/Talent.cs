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

        [MaxLength(15)]
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Illegal format phone number")]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(150)]
        [Required(ErrorMessage = "State is required")]  
        public string State { get; set; } = string.Empty;

        [MaxLength(150)]
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = string.Empty;
        [Url(ErrorMessage = "Invalid URL format")]
        [MaxLength(2500)]
        public string? GitHubUrl { get; set; } = string.Empty;

        [MaxLength(3000)]
        public string? ProfessionalSummary { get; set; } = string.Empty;

        public Education? Education { get; set; } = new Education();
        public Experience? Experience { get; set; } = new Experience();
        [Required]
        public List<String> Skills { get; set; } = new List<string>();
        [Required]
        public List<String> Languages { get; set; } = new List<string>();
        public List<URL>? AdditionalURL { get; set; } = new List<URL>();    
        public List<Certifications>? Certifications { get; set; } = new List<Certifications>();
    }
}
