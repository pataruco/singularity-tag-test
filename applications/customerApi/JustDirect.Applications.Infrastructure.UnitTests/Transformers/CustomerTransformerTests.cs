using JustDirect.Applications.Domain.Entities;
using JustDirect.Applications.Infrastructure.Transformers;
using Libraries.Dynamics.DynamicsClient;
using Newtonsoft.Json;

namespace JustDirect.Applications.Infrastructure.UnitTests.Transformers;

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
            EMailAddress1 = "john.doe@gmail.com",
            universe_Auth0ID = "random_id"
        };

        // act
        var result = CustomerTransformer.FromContact(contact);

        // assert
        var serializedResult = JsonConvert.SerializeObject(result);
        var serializedExpectedResult = JsonConvert.SerializeObject(expectedCustomer);
        Assert.That(serializedResult, Is.EqualTo(serializedExpectedResult));
    }

    [Test]
    public void MergeCustomerToMutableContact_UpdatesMutableFields_WhenCustomerFieldsAreNotNull()
    {
        // arrange
        var contact = new Contact()
        {
            Salutation = "Mr.",
            FirstName = "John",
            LastName = "Doe"
        };
        var customer = new Customer()
        {
            ContactId = Guid.NewGuid(),
            Email = "jane.smith@example.com",
            Salutation = "Dr.",
            FirstName = "Jane",
            LastName = "Smith"
        };

        var expectedContact = new Contact()
        {
            Salutation = "Dr.",
            FirstName = "Jane",
            LastName = "Smith"
        };

        // act
        var result = CustomerTransformer.MergeCustomerToMutableContact(contact, customer);

        // assert
        var serializedResult = JsonConvert.SerializeObject(result);
        var serializedExpectedResult = JsonConvert.SerializeObject(expectedContact);
        Assert.That(serializedResult, Is.EqualTo(serializedExpectedResult));
    }

    [Test]
    public void MergeCustomerToMutableContact_DoesNotUpdateFields_WhenCustomerFieldsAreNull()
    {
        // arrange
        var contact = new Contact()
        {
            Salutation = "Mr.",
            FirstName = "John",
            LastName = "Doe"
        };
        var customer = new Customer()
        {
            ContactId = Guid.NewGuid(),
            Email = "john.doe@example.com",
            Salutation = null,
            FirstName = null,
            LastName = null
        };

        var expectedContact = new Contact()
        {
            Salutation = "Mr.",
            FirstName = "John",
            LastName = "Doe"
        };

        // act
        var result = CustomerTransformer.MergeCustomerToMutableContact(contact, customer);

        // assert
        var serializedResult = JsonConvert.SerializeObject(result);
        var serializedExpectedResult = JsonConvert.SerializeObject(expectedContact);
        Assert.That(serializedResult, Is.EqualTo(serializedExpectedResult));
    }

    [Test]
    public void MergeCustomerToMutableContact_PartiallyUpdatesFields_WhenSomeCustomerFieldsAreNull()
    {
        // arrange
        var contact = new Contact()
        {
            Salutation = "Ms.",
            FirstName = "Alice",
            LastName = "Johnson"
        };
        var customer = new Customer()
        {
            ContactId = Guid.NewGuid(),
            Email = "alice.johnson@example.com",
            Salutation = null,
            FirstName = "Alicia",
            LastName = null
        };

        var expectedContact = new Contact()
        {
            Salutation = "Ms.",
            FirstName = "Alicia",
            LastName = "Johnson"
        };

        // act
        var result = CustomerTransformer.MergeCustomerToMutableContact(contact, customer);

        // assert
        var serializedResult = JsonConvert.SerializeObject(result);
        var serializedExpectedResult = JsonConvert.SerializeObject(expectedContact);
        Assert.That(serializedResult, Is.EqualTo(serializedExpectedResult));
    }
}