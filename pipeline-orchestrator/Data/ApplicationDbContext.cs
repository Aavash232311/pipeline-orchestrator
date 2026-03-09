namespace pipeline_orchestrator.Data;
using Microsoft.EntityFrameworkCore;
using pipeline_orchestrator.Model;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {}
    public DbSet<Talent> talent { get; set; } 
}

