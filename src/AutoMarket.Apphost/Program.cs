var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("postgres", port: 5432)
    .WithPgAdmin();

var automarketdb = db.AddDatabase("automarketdb");

var autoMarketApi = builder.AddProject<Projects.AutoMarket_API>("automarket-api")
    .WithReference(automarketdb);

builder.AddProject<Projects.AutoMarket_DatabaseMigration>("dbMigration")
    .WithReference(automarketdb);

builder.AddProject<Projects.AutoMarket_WebApp>("webApp")
    .WithExternalHttpEndpoints()
    .WithReference(autoMarketApi);

builder.Build().Run();
