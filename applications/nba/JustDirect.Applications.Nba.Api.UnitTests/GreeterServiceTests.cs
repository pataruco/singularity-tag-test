using JustDirect.Applications.Nba.Api.Services.V1;
using JustDirect.Applications.Nba.Api.UnitTests.Helpers;
using Libraries.Nba.GrpcContracts.V1;

namespace JustDirect.Applications.Nba.Api.UnitTests;

public class GreeterServiceTests
{
    private GreeterService _service;

    [SetUp]
    public void Setup()
    {
        _service = new GreeterService();
    }

    [Test]
    public async Task SayHello_ShouldReturnExpectedMessage()
    {
        // Arrange
        var request = new HelloRequest { Name = "Zahra" };
        var context = TestServerCallContext.Create();

        // Act
        var response = await _service.SayHello(request, context);

        // Assert
        Assert.That(response.Message, Is.EqualTo("Hello, Zahra!"));
    }

    [Test]
    public async Task SayGoodbye_ShouldReturnExpectedMessage()
    {
        // Arrange
        var request = new GoodbyeRequest { Name = "Zahra" };
        var context = TestServerCallContext.Create();

        // Act
        var response = await _service.SayGoodbye(request, context);

        // Assert
        Assert.That(response.Message, Is.EqualTo("Goodbye, Zahra!"));
    }
}