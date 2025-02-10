using Application.Api.Filters;
using Application.Api.Queries;
using Application.Core.Services.Interfaces;
using Application.Domain.Entities;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Snapshooter.NUnit;

namespace Application.Api.IntegrationTest.Queries;

public class Query_GetCustomer
{
    private Mock<ICustomerService> _mockCustomerService;
    private IRequestExecutorBuilder _requestExecutor;
    private Customer _customer;

    [SetUp]
    public void Setup()
    {
        _mockCustomerService = new Mock<ICustomerService>();
        _requestExecutor = new ServiceCollection()
            .AddSingleton(_mockCustomerService.Object)
            .AddGraphQLServer()
            .AddErrorFilter<CustomErrorFilter>()
            .AddQueryType<Query>();
        _customer = new Customer
        {
            UserId = "44248471-13bc-44ad-bc62-c0e07c4cfbbd",
            ContactId = "cab0c96e-7acc-4edf-8d4e-abfc3aa7c8cf",
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@gmail.com",
        };
    }

    [TestCase("defined", "defined", "defined")]
    [TestCase("defined", null, "defined")]
    [TestCase("defined", "defined", null)]
    [TestCase(null, "defined", "defined")]
    public async Task Customer_ThrowsIfMoreThanOneIdProvided(
        string? id,
        string? userId,
        string? contactId
    )
    {
        // arrange
        var query =
            $"{{ customer (where: {{ id: \"{id}\", userId: \"{userId}\", contactId: \"{contactId}\" }}){{ id }} }}";

        // act
        var result = await _requestExecutor.ExecuteRequestAsync(
            query);

        // assert
        result.ToJson().MatchSnapshot();
    }

    [Test]
    public async Task Customer_ThrowsWhenNoIdProvided()
    {
        // arrange
        var query =
            $"{{ customer (where: {{  }}){{ id }} }}";

        // act
        var result = await _requestExecutor.ExecuteRequestAsync(
            query);

        // assert
        result.ToJson().MatchSnapshot();
    }

    [Test]
    public async Task Customer_ReturnsWhenOnlyIdProvided()
    {
        // arrange
        var query =
            $"{{ customer (where: {{ id: \"Id\" }}){{ id, userId, contactId, firstName, lastName, email }} }}";
        _mockCustomerService.Setup(mock => mock.GetByUserId(It.IsAny<string>())).Returns(_customer);

        // act
        var result = await _requestExecutor.ExecuteRequestAsync(
            query);

        // assert
        result.ToJson().MatchSnapshot();
        _mockCustomerService.Verify(mock => mock.GetByUserId(It.Is<string>(val => val == "Id")), Times.Once);
    }

    [Test]
    public async Task Customer_ReturnsWhenOnlyUserIdProvided()
    {
        // arrange
        var query =
            $"{{ customer (where: {{ userId: \"User\" }}){{ id, userId, contactId, firstName, lastName, email }} }}";
        _mockCustomerService.Setup(mock => mock.GetByUserId(It.IsAny<string>())).Returns(_customer);


        // act
        var result = await _requestExecutor.ExecuteRequestAsync(
            query);

        // assert
        result.ToJson().MatchSnapshot();
        _mockCustomerService.Verify(mock => mock.GetByUserId(It.Is<string>(val => val == "User")), Times.Once);
    }

    [Test]
    public async Task Customer_ReturnsWhenOnlyContactIdProvided()
    {
        // arrange
        var query =
            $"{{ customer (where: {{ contactId: \"Contact\" }}){{ id, userId, contactId, firstName, lastName, email }} }}";
        _mockCustomerService.Setup(mock => mock.GetByContactId(It.IsAny<string>())).Returns(_customer);

        // act
        var result = await _requestExecutor.ExecuteRequestAsync(
            query);

        // assert
        result.ToJson().MatchSnapshot();
        _mockCustomerService.Verify(mock => mock.GetByContactId(It.Is<string>(val => val == "Contact")), Times.Once);
    }

    [Test]
    public async Task Customer_ReturnsNullWhenCustomerDoesNotExistForUserId()
    {
        // arrange
        var query =
            $"{{ customer (where: {{ userId: \"User\" }}){{ id, userId, contactId, firstName, lastName, email }} }}";
        _mockCustomerService.Setup(mock => mock.GetByUserId(It.IsAny<string>())).Returns((Customer?)null);

        // act
        var result = await _requestExecutor.ExecuteRequestAsync(
            query);

        // assert
        result.ToJson().MatchSnapshot();
    }

    [Test]
    public async Task Customer_ReturnsNullWhenCustomerDoesNotExistForContactId()
    {
        // arrange
        var query =
            $"{{ customer (where: {{ contactId: \"Contact\" }}){{ id, userId, contactId, firstName, lastName, email }} }}";
        _mockCustomerService.Setup(mock => mock.GetByContactId(It.IsAny<string>())).Returns((Customer?)null);

        // act
        var result = await _requestExecutor.ExecuteRequestAsync(
            query);

        // assert
        result.ToJson().MatchSnapshot();
    }
}