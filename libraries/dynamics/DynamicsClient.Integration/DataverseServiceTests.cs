
using Libraries.Dynamics.DynamicsClient.Extensions;
using Libraries.Dynamics.DynamicsClient.Models;
using Libraries.Dynamics.DynamicsClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Libraries.Dynamics.DynamicsClient.Integration.Tests;

public class DataverseServiceTests
{
    private IDataverseService _dataverseService;
    private Mock<ILogger<DataverseService>> _loggerMock;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(TestContext.CurrentContext.TestDirectory) // Ensure the correct path
            .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        _loggerMock = new Mock<ILogger<DataverseService>>();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ILogger<DataverseService>>(_loggerMock.Object);
        serviceCollection.AddDynamicsClient(configuration);


        var serviceProvider = serviceCollection.BuildServiceProvider();
        _dataverseService = serviceProvider.GetRequiredService<IDataverseService>();
    }

    [Test]
    public async Task WhoAmI_ShouldReturnWhoAmIResponse()
    {
        // Act
        var response = await _dataverseService.GetWhoAmIAsync();

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.UserId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(response.BusinessUnitId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(response.OrganizationId, Is.Not.EqualTo(Guid.Empty));
    }
}
