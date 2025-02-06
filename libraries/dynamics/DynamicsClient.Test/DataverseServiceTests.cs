
using Libraries.Dynamics.DynamicsClient.Factories;
using Libraries.Dynamics.DynamicsClient.Models;
using Libraries.Dynamics.DynamicsClient.Services;
using Libraries.Dynamics.DynamicsClient.Utilities;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;
using Moq;

namespace Libraries.Dynamics.DynamicsClient.Tests;

public class DataverseServiceTests
{
    private Mock<ITokenProvider> _tokenProviderMock;
    private Mock<ILogger<DataverseService>> _loggerMock;
    private Mock<IServiceClientFactory> _serviceClientFactoryMock;
    private Mock<IOrganizationServiceAsync2> _serviceClientMock;
    private Mock<IOptions<DynamicsOptions>> _optionsMock;
    private DataverseService _dataverseService;

    [SetUp]
    public void SetUp()
    {
        _tokenProviderMock = new Mock<ITokenProvider>();
        _loggerMock = new Mock<ILogger<DataverseService>>();
        _optionsMock = new Mock<IOptions<DynamicsOptions>>();
        _serviceClientFactoryMock = new Mock<IServiceClientFactory>();
        _serviceClientMock = new Mock<IOrganizationServiceAsync2>();

        var options = new DynamicsOptions
        {
            BaseUrl = "https://example.com",
            ClientId = "client-id",
            ClientSecret = "client-secret",
            TenantId = "tenant-id"
        };

        _optionsMock.Setup(o => o.Value).Returns(options);
        _serviceClientFactoryMock.Setup(factory => factory.CreateServiceClient(It.IsAny<Uri>(),
                It.IsAny<Func<string, Task<string>>>(), It.IsAny<bool>(), It.IsAny<ILogger<DataverseService>>()))
            .Returns(_serviceClientMock.Object);

        _dataverseService = new DataverseService(_serviceClientFactoryMock.Object, _tokenProviderMock.Object, _loggerMock.Object, _optionsMock.Object);
    }

    [Test]
    public async Task GetWhoAmIAsync_ShouldReturnWhoAmIResponse()
    {
        // Arrange
        var expectedToken = "test-token";
        var expectedUserId = Guid.NewGuid();
        var expectedBusinessUnitId = Guid.NewGuid();
        var expectedOrganizationId = Guid.NewGuid();

        _tokenProviderMock.Setup(ts => ts.GetAccessToken(It.IsAny<string>()))
            .ReturnsAsync(expectedToken);
        
        var whoAmIResponse = new WhoAmIResponse();
        whoAmIResponse.Results["BusinessUnitId"] = expectedBusinessUnitId;
        whoAmIResponse.Results["OrganizationId"] = expectedOrganizationId;
        whoAmIResponse.Results["UserId"] = expectedUserId;

        _serviceClientMock.Setup(sc => sc.ExecuteAsync(It.IsAny<WhoAmIRequest>()))
            .ReturnsAsync(whoAmIResponse);

        // Act
        var response = await _dataverseService.GetWhoAmIAsync();

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.UserId, Is.EqualTo(expectedUserId));
        Assert.That(response.BusinessUnitId, Is.EqualTo(expectedBusinessUnitId));
        Assert.That(response.OrganizationId, Is.EqualTo(expectedOrganizationId));
    }

    [Test]
    public void GetWhoAmIAsync_ShouldThrowException_WhenServiceClientFails()
    {
        // Arrange
        var expectedToken = "test-token";

        _tokenProviderMock.Setup(ts => ts.GetAccessToken(It.IsAny<string>()))
            .ReturnsAsync(expectedToken);

        _serviceClientMock.Setup(sc => sc.ExecuteAsync(It.IsAny<WhoAmIRequest>()))
            .ThrowsAsync(new Exception("Service client failed"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _dataverseService.GetWhoAmIAsync());
    }
}
