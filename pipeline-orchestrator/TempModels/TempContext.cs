using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace pipeline_orchestrator.TempModels;

public partial class TempContext : DbContext
{
    public TempContext()
    {
    }

    public TempContext(DbContextOptions<TempContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ScoringWeight> ScoringWeights { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ScoringWeight>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.WeightActiveContribs).HasColumnName("Weight_ActiveContribs");
            entity.Property(e => e.WeightCertifications).HasColumnName("Weight_Certifications");
            entity.Property(e => e.WeightEducation).HasColumnName("Weight_Education");
            entity.Property(e => e.WeightExperience).HasColumnName("Weight_Experience");
            entity.Property(e => e.WeightSkillDepth).HasColumnName("Weight_SkillDepth");
            entity.Property(e => e.WeightSkillMatch).HasColumnName("Weight_SkillMatch");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
