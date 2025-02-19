using Application.Domain.Entities;
using Application.Infrastructure.Transformers;
using Libraries.Dynamics.DynamicsClient;
using Newtonsoft.Json;

namespace Application.Infrastructure.UnitTest.Transformers;

public class CustomerTransformerTests
{
    [Test]
    public void FromContact_CreatesCustomerWithCorrectFieldsPopulated()
    {
        // arrange
        var guid = Guid.NewGuid();
        var expectedCustomer = new Customer()
        {
            ContactId = guid,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@gmail.com",
            UserId = "random_id"
        };
        var contact = new Contact()
        {
            Id = guid,
            FirstName = "John",
            LastName = "Doe",
            EmailAddress1 = "john.doe@gmail.com",
            New_Auth0Id = "random_id"
        };
        var transformer = new CustomerTransformer();

        // act
        var result = transformer.FromContact(contact);

        // assert
        var serializedResult = JsonConvert.SerializeObject(result);
        var serializedExpectedResult = JsonConvert.SerializeObject(expectedCustomer);
        Assert.That(serializedResult, Is.EqualTo(serializedExpectedResult));
    }
}