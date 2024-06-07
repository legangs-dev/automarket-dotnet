namespace AutoMarket.API.Applications.Vehicle.Queries;

public interface IVehicleQueries
{
    Task<Vehicle> GetVehicleAsync(Guid id);
}
