var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();
var db = postgres.AddDatabase("sqldb");

var apiService = builder.AddProject<Projects.CommodityPriceManager_ApiService>("apiservice")
    .WithReference(db)
    .WaitFor(db);

builder.AddProject<Projects.CommodityPriceManager_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
