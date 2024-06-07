using AutoMarket.Infrastructure.EntityConfiguration;

namespace AutoMarket.Infrastructure;

public class AutoMarketDbContext : DbContext
{
    public DbSet<Vehicle> Vehicles { get; set; }
    
    public AutoMarketDbContext(DbContextOptions<AutoMarketDbContext> options) : base(options)
    {
        System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VehicleEntityTypeConfiguration());
    }
}
