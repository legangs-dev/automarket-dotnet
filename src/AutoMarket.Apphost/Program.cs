var builder = DistributedApplication.CreateBuilder(args);

var automarketdb = builder.AddPostgres("postgres", port: 5432)
    .WithPgAdmin()
    .AddDatabase("automarketdb");

builder.AddProject<Projects.AutoMarket_API>("apiService")
    .WithReference(automarketdb);

builder.AddProject<Projects.AutoMarket_DatabaseMigration>("dbMigration")
    .WithReference(automarketdb);

builder.Build().Run();
