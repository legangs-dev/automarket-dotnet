using System.Diagnostics;
using AutoMarket.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;

namespace AutoMarket.DatabaseMigration;

public class Worker(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    private static readonly ActivitySource _activitySource = new("Migrations");
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = _activitySource.CreateActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AutoMarketDbContext>();
            await EnsureDatabaseAsync(dbContext, cancellationToken);
            await MigrateDatabaseAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(AutoMarketDbContext autoMarketDbContext, CancellationToken cancellationToken)
    {
        var dbCreator = autoMarketDbContext.GetService<IRelationalDatabaseCreator>();
        var strategy = autoMarketDbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task MigrateDatabaseAsync(AutoMarketDbContext autoMarketDbContext, CancellationToken cancellationToken)
    {
        var strategy = autoMarketDbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await autoMarketDbContext.Database.MigrateAsync(cancellationToken);
        });
    }
}
