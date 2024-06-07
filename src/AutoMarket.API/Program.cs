using AutoMarket.API.Applications.Exceptions;
using AutoMarket.API.Endpoints;
using AutoMarket.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

var app = builder.Build();

app.MapDefaultEndpoints();

app.NewVersionedApi("AutoMarket")
    .MapVehicleEndpointsV1();

app.UseExceptionHandler();
app.UseDefaultOpenApi();

app.Run();
