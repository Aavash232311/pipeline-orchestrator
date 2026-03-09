using System.ComponentModel.DataAnnotations;

namespace pipeline_orchestrator.Model
{
    public class Education
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(150)]    
        public string? Institution { get; set; } = string.Empty;
        public decimal GPA { get; set; } = 0;
    }
}
