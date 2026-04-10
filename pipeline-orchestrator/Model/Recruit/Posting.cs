using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pipeline_orchestrator.Model.Recruit
{
    public class Posting
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        public List<string>? RequiredSkills { get; set; } = new List<string>();
        public List<string>? PreferredSkills { get; set; } = new List<string>();
        public List<string>? RequiredLanguages { get; set; } = new List<string>();
        public Listing? Listing { get; set; } = new Listing();
        public int MinExperienceYears { get; set; } = 0;
        public int MaxExperienceYears { get; set; } = 10;
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        [MaxLength(50)]
        public string? Currency { get; set; } = "USD";
        public ScoringWeights Weights { get; set; } = new ScoringWeights();
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
        /* Just like me getting rejected, recruiter might need to know for how long candiate has been in college */
        public int? YearInCollege { get; set; } = 0;
        public bool? HasGraduated { get; set; } = false;

    }

    public class ScoringWeights
    {
        public Guid Id { get; set; }
        [Column("Weight_SkillMatch")]
        public decimal SkillMatch { get; set; } = 0.45m;
        [Column("Weight_SkillDepth")]
        public decimal SkillDepth { get; set; } = 0.25m;
        [Column("Weight_Experience")]
        public decimal Experience { get; set; } = 0.15m;
        [Column("Weight_Education")]
        public decimal Education { get; set; } = 0.10m;
        [Column("Weight_Certifications")]
        public decimal Certifications { get; set; } = 0.05m;
        [Column("Weight_ActiveContribs")]
        public decimal ActiveContributions { get; set; } = 0.05m;
    }
}
