using AutoMarket.DatabaseMigration;
using AutoMarket.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.AddServiceDefaults();
builder.Services.AddDbContextPool<AutoMarketDbContext>((options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("automarketdb"), sqlOptions =>
    {
        sqlOptions.ExecutionStrategy(c => new NpgsqlRetryingExecutionStrategy(c));
    });
});
builder.EnrichNpgsqlDbContext<AutoMarketDbContext>(settings => settings.DisableRetry = true);

var host = builder.Build();
host.Run();
