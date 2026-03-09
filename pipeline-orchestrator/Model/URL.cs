using System.ComponentModel.DataAnnotations;

namespace pipeline_orchestrator.Model
{
    public class URL
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(2500)]
        public string? URLString { get; set; } = string.Empty;
    }
}
