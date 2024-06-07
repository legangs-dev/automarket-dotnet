using AutoMarket.API.Applications.Vehicle.Commands;
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
        api.MapPost("/", CreateVehicleAsync);

        return api;
    }

    private static async Task CreateVehicleAsync(CreateVehicleCommand createVehicleCommand,
        [FromServices] IMediator mediator)
    {
        await mediator.Send(createVehicleCommand);
    }

    public static async Task<Results<Ok<Vehicle>, NotFound>> GetVehicleAsync(Guid vehicleId,
        [FromServices] IVehicleQueries vehicleQueries)
    {
        var vehicle = await vehicleQueries.GetVehicleAsync(vehicleId);
        return TypedResults.Ok(vehicle);
    }
}
