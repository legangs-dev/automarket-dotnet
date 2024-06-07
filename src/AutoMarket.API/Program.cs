using AutoMarket.API.Endpoints;
using AutoMarket.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();

var withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

var app = builder.Build();

app.MapDefaultEndpoints();

app.NewVersionedApi("AutoMarket")
    .MapVehicleEndpointsV1();

app.UseDefaultOpenApi();

app.Run();
