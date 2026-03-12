using System.ComponentModel.DataAnnotations;

namespace pipeline_orchestrator.Model
{
    public class Listing
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; } = string.Empty;
        public bool IsRemote { get; set; } = false;
        [Required]
        [MaxLength(255)]
        public string Country { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string City { get; set; } = string.Empty;
        [Required]
        public decimal Lat { get; set; } = 0;
        [Required]
        public decimal Long { get; set; } = 0;
        public bool RelocateAvailable { get; set; } = false;
        public List<String> Languages { get; set; } = new List<String>();

    }
}
