namespace Varuna.Presentation.Rest.Tests;

public sealed class RestApplicationFactory : IAsyncInitialization, IAsyncDisposable
{
    public RestApplicationFactory()
    {
        Initialization = InitializeAsync();
    }

    public Task Initialization { get; private set; }

    public DistributedApplication? DistributedApplication { get; private set; }
    public ResourceNotificationService? ResourceNotificationService { get; private set; }

    private async Task InitializeAsync()
    {
        var appHost =
            await DistributedApplicationTestingBuilder.CreateAsync<Projects.Varuna_AppHost>()
                .ConfigureAwait(false);

        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
                                                     {
                                                         clientBuilder.AddStandardResilienceHandler();
                                                     });
        // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging

        DistributedApplication = await appHost.BuildAsync().ConfigureAwait(false);
        ResourceNotificationService = DistributedApplication.Services.GetRequiredService<ResourceNotificationService>();
        await DistributedApplication.StartAsync().ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (Initialization is IAsyncDisposable initializationAsyncDisposable)
            await initializationAsyncDisposable.DisposeAsync();
        else
            Initialization.Dispose();

        if (DistributedApplication != null)
        {
            await DistributedApplication.DisposeAsync();
        }
    }
}
