using AutoMarket.API.Applications.Vehicle.Queries;
using AutoMarket.Infrastructure;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<AutoMarketDbContext>("automarketdb");

        builder.Services.AddScoped<IVehicleQueries, VehicleQueries>();
    }
}
