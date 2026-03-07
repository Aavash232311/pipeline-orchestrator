using System.ComponentModel.DataAnnotations;

namespace pipeline_orchestrator.Model
{
    public class Education
    {
        [MaxLength(150)]    
        public string? Institution { get; set; } = string.Empty;
        public decimal GPA { get; set; } = 0;
    }
}
