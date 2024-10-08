namespace Varuna.Presentation.Rest.Tests;

[Collection(nameof(RestCollectionDefinition))]
public sealed class ControllerTests(RestApplicationFactory factory)
{
    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
#pragma warning disable VSTHRD003
        await factory.Initialization;
#pragma warning restore VSTHRD003
        // Act
        var httpClient = factory.DistributedApplication?.CreateHttpClient("webfrontend");
        await factory.ResourceNotificationService?
            .WaitForResourceAsync("webfrontend", KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromSeconds(30))!;

        var response = await httpClient?.GetAsync("/")!;

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
