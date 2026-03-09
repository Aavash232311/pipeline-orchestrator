using System.ComponentModel.DataAnnotations;

namespace pipeline_orchestrator.Model
{
    public class Certifications
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(250)]
        public string? CertificationName { get; set; } = string.Empty;
        [MaxLength(250)]
        public string? IssuingOrganization { get; set; } = string.Empty;
        public DateTime? IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpirationDate { get; set; } = DateTime.UtcNow;
        [MaxLength(2500)]
        public string? CredentialURL { get; set; } = string.Empty;
    }
}
