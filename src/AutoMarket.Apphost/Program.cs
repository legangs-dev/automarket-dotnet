var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("postgres", port: 5432)
    .WithPgAdmin();

var automarketdb = db.AddDatabase("automarketdb");

builder.AddProject<Projects.AutoMarket_API>("apiService")
    .WithReference(automarketdb);

builder.AddProject<Projects.AutoMarket_DatabaseMigration>("dbMigration")
    .WithReference(automarketdb);

builder.Build().Run();
