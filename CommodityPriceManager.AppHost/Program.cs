var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();
var db = postgres.AddDatabase("sqldb");

var apiService = builder.AddProject<Projects.CommodityPriceManager_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(db)
    .WaitFor(db);

builder.AddContainer("webfrontend", "commodity-webfrontend")
    .WithDockerfile("../CommodityPriceManager.Web")
    .WithHttpEndpoint(port: 80, targetPort: 80, name: "http")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();