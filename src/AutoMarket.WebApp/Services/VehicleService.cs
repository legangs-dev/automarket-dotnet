
namespace AutoMarket.WebApp.Services;

public class VehicleService(HttpClient httpClient)
{
    public async Task<bool> GetVehicleAsync()
    {
        var uri = $"api/vehicle/{Guid.NewGuid()}";
        await httpClient.GetAsync(uri);

        return true;
    }
}
