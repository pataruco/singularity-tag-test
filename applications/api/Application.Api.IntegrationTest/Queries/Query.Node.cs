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

namespace Application.Api.IntegrationTest.Queries
{
    public class Query_Node
    {

        private Mock<ICustomerService> _mockCustomerService;
        private IRequestExecutorBuilder _requestExecutor;
        private Customer _customer;
        private Guid _customerId;

        [SetUp]
        public void Setup()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _requestExecutor = new ServiceCollection()
                .AddSingleton(_mockCustomerService.Object)
                .AddGraphQLServer()
                .AddGlobalObjectIdentification()
                .AddErrorFilter<CustomErrorFilter>()
                .AddQueryType<Query>();
            _customerId = Guid.Parse("cab0c96e-7acc-4edf-8d4e-abfc3aa7c8cf");
            _customer = new Customer
            {
                UserId = "44248471-13bc-44ad-bc62-c0e07c4cfbbd",
                ContactId = _customerId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
            };
        }

        [Test]
        public async Task Node_GetCustomerThrowsWhenIdProvidedThatIsNotAnEncodedGuid()
        {
            // arrange
            var query =
                $"{{ node (id: \"Q3VzdG9tZXI6Y2FiMGM5NmUtN2FjYy00ZWRmLThkNGUtYWJmYzNhYTdjOGN\"){{ ... on Customer {{ id }} }} }}";

            // act
            var result = await _requestExecutor.ExecuteRequestAsync(
                query);

            // assert
            result.ToJson().MatchSnapshot();
        }

        [Test]
        public async Task Node_GetCustomerReturnsCustomerIfFound()
        {
            // arrange
            var query =
                $"{{ node (id: \"Q3VzdG9tZXI6Y2FiMGM5NmUtN2FjYy00ZWRmLThkNGUtYWJmYzNhYTdjOGNm\"){{ ...on Customer {{ id, userId, contactId, firstName, lastName, email }} }} }}";
            _mockCustomerService.Setup(mock => mock.GetByContactId(It.IsAny<Guid>())).Returns(_customer);

            // act
            var result = await _requestExecutor.ExecuteRequestAsync(
                query);

            // assert
            result.ToJson().MatchSnapshot();
            _mockCustomerService.Verify(mock => mock.GetByContactId(It.Is<Guid>(val => val == _customerId)), Times.Once);
        }

        [Test]
        public async Task Node_GetCustomerReturnsNullIfCustomerNotFound()
        {
            // arrange
            var query =
                $"{{ node (id: \"Q3VzdG9tZXI6Y2FiMGM5NmUtN2FjYy00ZWRmLThkNGUtYWJmYzNhYTdjOGNm\"){{ ...on Customer {{ id, userId, contactId, firstName, lastName, email }} }} }}";
            _mockCustomerService.Setup(mock => mock.GetByContactId(It.IsAny<Guid>())).Returns((Customer?)null);

            // act
            var result = await _requestExecutor.ExecuteRequestAsync(
                query);

            // assert
            result.ToJson().MatchSnapshot();
            _mockCustomerService.Verify(mock => mock.GetByContactId(It.Is<Guid>(val => val == _customerId)), Times.Once);
        }
    }
}