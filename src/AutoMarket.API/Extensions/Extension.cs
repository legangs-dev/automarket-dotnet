using AutoMarket.API.Applications.Behaviors;
using AutoMarket.API.Applications.Vehicle.Commands;
using AutoMarket.API.Applications.Vehicle.Queries;
using AutoMarket.Infrastructure;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<AutoMarketDbContext>("automarketdb");

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        builder.Services.AddSingleton<IValidator<CreateVehicleCommand>, CreateVehicleCommandValidator>();

        builder.Services.AddScoped<IVehicleQueries, VehicleQueries>();
    }

    public static string GetGenericTypeName(this Type type)
    {
        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name));
            return $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }

        return type.Name;
    }

    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }
}
