
using Libraries.Dynamics.DynamicsClient.Extensions;
using Libraries.Dynamics.DynamicsClient.Models;
using Libraries.Dynamics.DynamicsClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Libraries.Dynamics.DynamicsClient.Integration.Tests;

public class TokenServiceTests
{
    private ITokenService _tokenService;
    private Mock<ILogger<TokenService>> _loggerMock;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(TestContext.CurrentContext.TestDirectory) // Ensure the correct path
            .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        _loggerMock = new Mock<ILogger<TokenService>>();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ILogger<TokenService>>(_loggerMock.Object);
        serviceCollection.AddDynamicsClient(configuration);


        var serviceProvider = serviceCollection.BuildServiceProvider();
        _tokenService = serviceProvider.GetRequiredService<ITokenService>();
    }

    [Test]
    public async Task GetAccessTokenAsync_ShouldReturnToken()
    {
        // Act
        var response = await _tokenService.GetAccessTokenAsync(CancellationToken.None);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.AccessToken, Is.Not.Null);
    }

    [Test]
    public async Task GetAccessTokenAsync_ShouldReturnCachedToken()
    {
        // Act
        var response1 = await _tokenService.GetAccessTokenAsync(CancellationToken.None);
        var response2 = await _tokenService.GetAccessTokenAsync(CancellationToken.None);

        // Assert
        Assert.That(response1.AccessToken, Is.EqualTo(response2.AccessToken));
    }
}
