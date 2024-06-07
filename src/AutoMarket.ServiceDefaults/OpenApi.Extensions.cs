using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoMarket.ServiceDefaults;

public static partial class Extensions
{
    public static IHostApplicationBuilder AddDefaultOpenApi(this IHostApplicationBuilder builder,
        IApiVersioningBuilder? apiVersioning = default)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        var openApi = configuration.GetSection("OpenApi");

        if (openApi.Exists())
        {
            services.AddEndpointsApiExplorer();

            if (apiVersioning is not null)
            {
                apiVersioning.AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");
                services.AddSwaggerGen(options => options.OperationFilter<OpenApiDefaultValues>());
            }
        }

        return builder;
    }

    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        var configuration = app.Configuration;
        var openApiSection = configuration.GetSection("OpenApi");

        if (!openApiSection.Exists())
        {
            return app;
        }

        app.UseSwagger();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(setup =>
            {
                /// {
                ///   "OpenApi": {
                ///     "Endpoint: {
                ///         "Name": 
                ///     },
                ///     "Auth": {
                ///         "ClientId": ..,
                ///         "AppName": ..
                ///     }
                ///   }
                /// }

                var pathBase = configuration["PATH_BASE"] ?? string.Empty;
                var endpointSection = openApiSection.GetRequiredSection("Endpoint");

                foreach (var description in app.DescribeApiVersions())
                {
                    var name = description.GroupName;
                    var url = endpointSection["Url"] ?? $"{pathBase}/swagger/{name}/swagger.json";

                    setup.SwaggerEndpoint(url, name);
                }                
            });

            // Add a redirect from the root of the app to the swagger endpoint
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
        }
        return app;
    }
}
