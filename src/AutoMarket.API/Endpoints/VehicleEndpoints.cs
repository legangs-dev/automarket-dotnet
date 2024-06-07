using AutoMarket.API.Applications.Vehicle.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AutoMarket.API.Endpoints;

public static class VehicleEndpoints
{
    public static RouteGroupBuilder MapVehicleEndpointsV1(this IEndpointRouteBuilder routeBuilder)
    {
        var api = routeBuilder.MapGroup("/api/vehicle").HasApiVersion(1.0);

        api.MapGet("/items", () => { return "hello"; });
        api.MapGet("{vehicleId:Guid}", GetVehicleAsync);

        return api;
    }

    public static async Task<Results<Ok<Vehicle>, NotFound>> GetVehicleAsync(Guid vehicleId, [FromServices] IVehicleQueries vehicleQueries)
    {
        var vehicle = await vehicleQueries.GetVehicleAsync(vehicleId);
        return TypedResults.Ok(vehicle);
    }
}
