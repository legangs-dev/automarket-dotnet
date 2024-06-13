using AutoMarket.WebApp.Services;

namespace AutoMarket.WebApp.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder hostApplicationBuilder)
    {
        hostApplicationBuilder.Services.AddHttpClient<VehicleService>(o => o.BaseAddress = new Uri("http://automarket-api"))
            .AddApiVersion(1.0);
    }
}
