using System;
using System.Collections.Generic;

namespace pipeline_orchestrator.Model;

public partial class ProgrammingLang
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Category { get; set; }

    public string? Type { get; set; }

    public string? Field { get; set; }

    public string? Ecosystem { get; set; }

    public int Difficulty { get; set; }

    public decimal? DifficultyNormalized { get; set; }
}
