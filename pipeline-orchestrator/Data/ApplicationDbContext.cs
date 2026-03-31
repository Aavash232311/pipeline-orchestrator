namespace pipeline_orchestrator.Data;
using Microsoft.EntityFrameworkCore;
using pipeline_orchestrator.Model;
using pipeline_orchestrator.Model.Recruit;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }
    public DbSet<Talent> talent { get; set; } 
    public DbSet<Listing> listing { get; set; }
    public DbSet<Posting> posting { get; set; }
    public virtual DbSet<ProgrammingLang> ProgrammingLangs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProgrammingLang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("programming_lang_pkey");

            entity.ToTable("programming_lang");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.Difficulty)
                .HasDefaultValue(1)
                .HasColumnName("difficulty");
            entity.Property(e => e.DifficultyNormalized)
                .HasPrecision(5, 4)
                .HasComputedColumnSql("((1.0)::double precision / ((1.0)::double precision + power(exp((1)::double precision), (('-0.8'::numeric * ((difficulty - 5))::numeric))::double precision)))", true)
                .HasColumnName("difficulty_normalized");
            entity.Property(e => e.Ecosystem)
                .HasMaxLength(255)
                .HasColumnName("ecosystem");
            entity.Property(e => e.Field)
                .HasMaxLength(255)
                .HasColumnName("field");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

