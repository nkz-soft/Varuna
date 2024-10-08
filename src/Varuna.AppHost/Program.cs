var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder.AddKeycloak("keycloak");

var apiService = builder.AddProject<Projects.Varuna_Presentation_Rest>("apiservice")
    .WithReference(keycloak);

builder.AddProject<Projects.Varuna_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(keycloak)
    .WithReference(apiService);

await builder.Build()
    .RunAsync()
    .ConfigureAwait(false);
