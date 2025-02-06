using Libraries.Dynamics.DynamicsClient.Factories;
using Libraries.Dynamics.DynamicsClient.Models;
using Libraries.Dynamics.DynamicsClient.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Xrm.Sdk;
using Moq;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Libraries.Dynamics.DynamicsClient.Test;

[TestFixture]
public class TokenServiceTests
{
    private Mock<IOptions<DynamicsOptions>> _optionsMock;
    private Mock<IConfidentialClientApplicationAdapter> _clientApplicationAdapterMock;
    private Mock<ILogger<TokenService>> _loggerMock;
    private TokenService _tokenService; // Using concrete class for testing all methods
    private DynamicsOptions _dynamicsOptions;
    private Mock<IConfidentialClientApplication> _confidentialClientMock;

    [SetUp]
    public void SetUp()
    {
        _dynamicsOptions = new DynamicsOptions
        {
            BaseUrl = "https://example.com",
            ClientId = "client-id",
            ClientSecret = "client-secret",
            TenantId = "tenant-id",
            AuthorityDomain = "https://login.microsoftonline.com/"
        };

        _optionsMock = new Mock<IOptions<DynamicsOptions>>();
        _optionsMock.Setup(o => o.Value).Returns(_dynamicsOptions);

        _loggerMock = new Mock<ILogger<TokenService>>();

        _confidentialClientMock = new Mock<IConfidentialClientApplication>();

        _clientApplicationAdapterMock = new Mock<IConfidentialClientApplicationAdapter>();

        _tokenService = new TokenService(_optionsMock.Object, _clientApplicationAdapterMock.Object, _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _tokenService.Dispose();
    }

    [Test]
    public async Task GetAccessTokenAsync_ReturnsCachedToken_WhenTokenIsValid()
    {
        // Arrange
        var account = new Mock<IAccount>();
        var cachedToken = new AuthenticationResult("access-token", false, "id-token",
            DateTimeOffset.UtcNow.AddMinutes(5), DateTimeOffset.UtcNow.AddMinutes(5), "tenant-id",
            account.Object, "username", ["scope"], Guid.NewGuid());
        
        _tokenService.GetType()
            .GetField("_cachedToken",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(_tokenService, cachedToken);
        
        // Act
        var result = await _tokenService.GetAccessTokenAsync();

        // Assert
        Assert.That(result, Is.EqualTo(cachedToken));
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Returning cached token.")),
                null,
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Test]
    public async Task GetAccessTokenAsync_AcquiresNewToken_WhenCachedTokenNotSet()
    {
        // Arrange
        var account = new Mock<IAccount>();
        var newToken = new AuthenticationResult("new-access-token", false, "id-token",
            DateTimeOffset.UtcNow.AddMinutes(5), DateTimeOffset.UtcNow.AddMinutes(5), "tenant-id",
            account.Object, "username", ["scope"], Guid.NewGuid());

        _clientApplicationAdapterMock
            .Setup(adapter => adapter.AcquireTokenForClientAsync(It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newToken);

        // Act
        var result = await _tokenService.GetAccessTokenAsync();

        // Assert
        Assert.That(result, Is.EqualTo(newToken));
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Token acquired successfully.")),
                null,
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Test]
    public async Task GetAccessTokenAsync_AcquiresNewToken_WhenCachedTokenExpired()
    {
        // Arrange
        var account = new Mock<IAccount>();
        var cachedToken = new AuthenticationResult("access-token", false, "id-token",
            DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddMinutes(5), "tenant-id",
            account.Object, "username", ["scope"], Guid.NewGuid());

        _tokenService.GetType()
            .GetField("_cachedToken",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(_tokenService, cachedToken);
        
        var newToken = new AuthenticationResult("new-access-token", false, "id-token",
            DateTimeOffset.UtcNow.AddMinutes(5), DateTimeOffset.UtcNow.AddMinutes(5), "tenant-id",
            account.Object, "username", ["scope"], Guid.NewGuid());

        _clientApplicationAdapterMock
            .Setup(adapter => adapter.AcquireTokenForClientAsync(It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newToken);

        // Act
        var result = await _tokenService.GetAccessTokenAsync();

        // Assert
        Assert.That(result, Is.EqualTo(newToken));
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Token acquired successfully.")),
                null,
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Test]
    public void GetAccessTokenAsync_ThrowsException_WhenMsalServiceExceptionOccurs()
    {
        // Arrange
        _clientApplicationAdapterMock
            .Setup(adapter => adapter.AcquireTokenForClientAsync(It.IsAny<string[]>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new MsalServiceException("error-code", "error-message"));

        // Act & Assert
        Assert.ThrowsAsync<MsalServiceException>(async () => await _tokenService.GetAccessTokenAsync());

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("An error occurred while acquiring the token.")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Test]
    public void Constructor_ThrowsArgumentNullException_WhenOptionsIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new TokenService(null, _clientApplicationAdapterMock.Object, _loggerMock.Object));
    }

    [Test]
    public void Constructor_ThrowsArgumentNullException_WhenFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new TokenService(_optionsMock.Object, null, _loggerMock.Object));
    }

    [Test]
    public void Constructor_ThrowsArgumentNullException_WhenLoggerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new TokenService(_optionsMock.Object, _clientApplicationAdapterMock.Object, null));
    }

    [Test]
    public void Constructor_ThrowsArgumentException_WhenOptionsAreInvalid()
    {
        var invalidOptions = new DynamicsOptions(); // Missing required properties
        _optionsMock.Setup(o => o.Value).Returns(invalidOptions);

        Assert.Throws<ArgumentException>(() => new TokenService(_optionsMock.Object, _clientApplicationAdapterMock.Object, _loggerMock.Object));
    }

    [Test]
    public void Dispose_DisposesSemaphore()
    {
        // Act
        _tokenService.Dispose();

        //  Act & Assert
        Assert.DoesNotThrow(() => _tokenService.Dispose()); // Second call should not throw
    }
}
