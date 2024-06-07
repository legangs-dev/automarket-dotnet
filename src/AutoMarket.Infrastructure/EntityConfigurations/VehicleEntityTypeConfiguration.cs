namespace AutoMarket.Infrastructure.EntityConfiguration;

public class VehicleEntityTypeConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");

        builder.Property(o => o.Id).HasValueGenerator<GuidValueGenerator>();
    }
}
