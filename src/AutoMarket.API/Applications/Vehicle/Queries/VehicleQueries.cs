using AutoMarket.Infrastructure;

namespace AutoMarket.API.Applications.Vehicle.Queries;

public class VehicleQueries : IVehicleQueries
{
    private readonly AutoMarketDbContext _autoMarketDbContext;

    public VehicleQueries(AutoMarketDbContext autoMarketDbContext)
    {
        _autoMarketDbContext = autoMarketDbContext;
    }
    public async Task<Vehicle> GetVehicleAsync(Guid id)
    {
        await _autoMarketDbContext.Vehicles.FirstOrDefaultAsync();
        return new Vehicle(id);
    }
}
