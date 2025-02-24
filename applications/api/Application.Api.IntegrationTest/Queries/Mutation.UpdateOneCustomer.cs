using Application.Api.Filters;
using Application.Api.Queries;
using Application.Core.Commands;
using Application.Core.Commands.Customer;
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
    public class Mutation_UpdateOneCustomer
    {
        private Mock<ICustomerService> _mockCustomerService;
        private Mock<ICommandHandler<UpdateCustomerCommand>> _mockUpdateCustomerCommandHandler;
        private IRequestExecutorBuilder _requestExecutor;
        private Customer _customer;
        private Guid _customerId;

        [SetUp]
        public void Setup()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _mockUpdateCustomerCommandHandler = new Mock<ICommandHandler<UpdateCustomerCommand>>();
            _requestExecutor = new ServiceCollection()
                .AddSingleton(_mockCustomerService.Object)
                .AddSingleton(_mockUpdateCustomerCommandHandler.Object)
                .AddGraphQLServer()
                .AddErrorFilter<CustomErrorFilter>()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>();
            _customerId = Guid.Parse("cab0c96e-7acc-4edf-8d4e-abfc3aa7c8cf");
            _customer = new Customer
            {
                UserId = "idp_123abc321",
                ContactId = _customerId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Salutation = "Mr."
            };
        }

        [Test]
        public async Task ReturnsNull_WhenCustomerNotFound()
        {
            // Arrange
            const string salutation = "Nickname";

            _mockCustomerService.Setup(mock => mock.GetByContactId(It.IsAny<Guid>())).Returns((Customer)null);

            var mutation = $@"
                mutation {{
                    updateOneCustomer(where: {{ contactId: ""{_customerId}"" }}, input: {{
                        salutation: ""{salutation}""
                    }}) {{
                        id,
                        userId,
                        contactId,
                        firstName,
                        lastName,
                        email,
                        salutation
                    }}
                }}";

            // Act
            var result = await _requestExecutor.ExecuteRequestAsync(mutation);

            // Assert
            result.ToJson().MatchSnapshot();

            _mockUpdateCustomerCommandHandler.Verify(mock => mock.Handle(It.Is<UpdateCustomerCommand>(c =>
                c.Id.ContactId == _customerId &&
                c.Salutation == salutation)), Times.Once);
        }

        [Test]
        public async Task UpdateCustomer_WhenValid()
        {
            // Arrange
            const string salutation = "Nickname";

            var expectedCustomer = _customer;
            expectedCustomer.Salutation = salutation;

            var mutation = $@"
                mutation {{
                    updateOneCustomer(where: {{ contactId: ""{_customerId}"" }}, input: {{
                        salutation: ""{salutation}""
                    }}) {{
                        id,
                        userId,
                        contactId,
                        firstName,
                        lastName,
                        email,
                        salutation
                    }}
                }}";

            _mockCustomerService.Setup(mock => mock.GetByContactId(It.IsAny<Guid>())).Returns(expectedCustomer);
            _mockUpdateCustomerCommandHandler.Setup(mock => mock.Handle(It.IsAny<UpdateCustomerCommand>()));

            // Act
            var result = await _requestExecutor.ExecuteRequestAsync(mutation);

            // Assert
            result.ToJson().MatchSnapshot();

            _mockUpdateCustomerCommandHandler.Verify(mock => mock.Handle(It.Is<UpdateCustomerCommand>(c =>
                c.Id.ContactId == _customerId &&
                c.Salutation == salutation)), Times.Once);
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
            // Arrange
            var mutation = $@"
                mutation {{
                    updateOneCustomer(where: {{ id: ""{id}"", userId: ""{userId}"", contactId: ""{contactId}"" }}, input: {{
                        salutation: ""Nickname""
                    }}) {{
                        id
                    }}
                }}";

            // Act
            var result = await _requestExecutor.ExecuteRequestAsync(mutation);

            // Assert
            result.ToJson().MatchSnapshot();
        }

        [Test]
        public async Task Customer_ThrowsWhenNoIdProvided()
        {
            // Arrange
            var mutation = @"
                mutation {{
                    updateOneCustomer(where: {{  }}, input: {{
                        salutation: ""Nickname""
                    }}) {{
                        id
                    }}
                }}";

            // Act
            var result = await _requestExecutor.ExecuteRequestAsync(mutation);

            // Assert
            result.ToJson().MatchSnapshot();
        }

        [Test]
        public async Task Customer_ThrowsWhenIdProvidedThatIsNotAGuid()
        {
            // Arrange
            var mutation =
                $"mutation {{ updateOneCustomer(where: {{ id: \"test\" }}, input: {{ salutation: \"Nickname\" }}) {{ id }} }}";

            // Act
            var result = await _requestExecutor.ExecuteRequestAsync(mutation);

            // Assert
            result.ToJson().MatchSnapshot();
        }

        [Test]
        public async Task Customer_ThrowsWhenContactIdProvidedThatIsNotAGuid()
        {
            // Arrange
            var mutation =
                $"mutation {{ updateOneCustomer(where: {{ contactId: \"test\" }}, input: {{ salutation: \"Nickname\" }}) {{ id }} }}";

            // Act
            var result = await _requestExecutor.ExecuteRequestAsync(mutation);

            // Assert
            result.ToJson().MatchSnapshot();
        }
    }
}