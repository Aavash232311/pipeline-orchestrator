using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pipeline_orchestrator.Model
{
    public class Listing
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("ListingAddress")]
        public string Address { get; set; } = string.Empty;
        public bool IsRemote { get; set; } = false;
        [Required]
        [Column("ListingCountry")]
        [MaxLength(255)]
        public string Country { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        [Column("ListingCity")]
        public string City { get; set; } = string.Empty;
        [Required]
        [Column("ListingLat")]
        public decimal Lat { get; set; } = 0;
        [Column("ListingLong")]
        [Required]
        public decimal Long { get; set; } = 0;
        public bool RelocateAvailable { get; set; } = false;
        public List<String>? Languages { get; set; } = new List<String>();

    }
}
