using System.ComponentModel.DataAnnotations;

namespace pipeline_orchestrator.Model
{
    public class URL
    {
        [MaxLength(2500)]
        public string? URLString { get; set; } = string.Empty;
    }
}
