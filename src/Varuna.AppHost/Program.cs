var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Varuna_ApiService>("apiservice");

builder.AddProject<Projects.Varuna_Web>("webfrontend")
    .WithReference(apiService);

builder.Build().Run();
