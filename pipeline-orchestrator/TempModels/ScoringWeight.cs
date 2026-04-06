using System;
using System.Collections.Generic;

namespace pipeline_orchestrator.TempModels;

public partial class ScoringWeight
{
    public Guid Id { get; set; }

    public decimal WeightSkillMatch { get; set; }

    public decimal WeightSkillDepth { get; set; }

    public decimal WeightExperience { get; set; }

    public decimal WeightEducation { get; set; }

    public decimal WeightCertifications { get; set; }

    public decimal WeightActiveContribs { get; set; }
}
