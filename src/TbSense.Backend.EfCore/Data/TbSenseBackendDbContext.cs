using Microsoft.EntityFrameworkCore;
using MasLazu.AspNet.Framework.EfCore.Data;
using TbSense.Backend.Domain.Entities;
using System.Reflection;

namespace TbSense.Backend.EfCore.Data;

public class TbSenseBackendDbContext : BaseDbContext
{
    public TbSenseBackendDbContext(DbContextOptions<TbSenseBackendDbContext> options) : base(options)
    {
    }

    public DbSet<Plantation> Plantations { get; set; }
    public DbSet<PlantationCoordinate> PlantationCoordinates { get; set; }
    public DbSet<Tree> Trees { get; set; }
    public DbSet<TreeMetric> TreeMetrics { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<PlantationYieldPrediction> PlantationYieldPredictions { get; set; }
    public DbSet<PlantationHarvest> PlantationHarvests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}