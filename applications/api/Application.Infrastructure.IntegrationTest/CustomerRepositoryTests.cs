using Application.Domain.Entities;
using Application.Infrastructure.Interfaces;
using Application.Infrastructure.Repositories;
using Libraries.Dynamics.DynamicsClient;
using Libraries.Dynamics.DynamicsClient.Extensions;
using Libraries.Dynamics.DynamicsClient.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;

namespace Application.Infrastructure.IntegrationTest;

[TestFixture]
[Ignore("Will reinstate once Dynamics instance setup")]
public class CustomerRepositoryTests
{
    private DataverseContext _context;
    private ICustomerRepository _customerRepository;
    private Contact _contact;
    private Customer _expectedCustomer;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(TestContext.CurrentContext.TestDirectory) // Ensure the correct path
            .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
            .Build();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDynamicsClient(configuration);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var contextFactory = serviceProvider.GetRequiredService<IDataverseContextFactory>();

        // Setup contact in dataverse
        _context = contextFactory.CreateDataverseContext();

        _contact = new Contact();
        _contact.FirstName = "John";
        _contact.LastName = "Doe";
        _contact.EmailAddress1 = "john.doe@gmail.com";
        _contact.New_Auth0Id = $"email|{_contact.EmailAddress1}";
        _contact.Salutation = "Mr.";

        _context.AddObject(_contact);
        _context.SaveChanges();

        _expectedCustomer = new Customer()
        {
            FirstName = _contact.FirstName,
            LastName = _contact.LastName,
            Email = _contact.EmailAddress1,
            UserId = _contact.New_Auth0Id,
            ContactId = _contact.Id,
            Salutation = _contact.Salutation
        };

        // Created a mocked IDataverseContextFactory to return the already instantiated context 
        // when our CustomerRepository we want to test is ran.
        var mockDataverseContextFactory = new Mock<IDataverseContextFactory>();
        mockDataverseContextFactory.Setup(f => f.CreateDataverseContext()).Returns(_context);

        _customerRepository = new CustomerRepository(mockDataverseContextFactory.Object);
    }

    [Test]
    public void Get_ThrowsNotImplementedException()
    {
        // act/assert
        Assert.Throws<NotImplementedException>(() => _customerRepository.Get());
    }

    [Test]
    public void GetById_ReturnsCustomerIfNotNull()
    {
        // act
        var customer = _customerRepository.GetById(_contact.Id);

        // assert
        var serializedCustomer = JsonConvert.SerializeObject(customer);
        var serializedExpectedCustomer = JsonConvert.SerializeObject(_expectedCustomer);

        Assert.That(serializedCustomer, Is.EqualTo(serializedExpectedCustomer));
        Assert.That(customer.ContactId, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void GetById_ReturnsNullIfCustomerDoesNotExist()
    {
        // act
        var customer = _customerRepository.GetById(Guid.Empty);

        // assert
        Assert.That(customer, Is.Null);
    }

    [Test]
    public void GetByAuth0Id_ReturnsCustomerIfNotNull()
    {
        // act
        var customer = _customerRepository.GetByAuth0Id(_contact.New_Auth0Id);

        // assert
        var serializedCustomer = JsonConvert.SerializeObject(customer);
        var serializedExpectedCustomer = JsonConvert.SerializeObject(_expectedCustomer);

        Assert.That(serializedCustomer, Is.EqualTo(serializedExpectedCustomer));
        Assert.That(customer.ContactId, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void GetByAuth0Id_ReturnsNullIfCustomerDoesNotExist()
    {
        // act
        var customer = _customerRepository.GetByAuth0Id("non_existant_auth0_id");

        // assert
        Assert.That(customer, Is.Null);
    }

    [Test]
    public void Update_UpdatesCustomerDetails()
    {
        // arrange
        var updatedCustomer = new Customer()
        {
            ContactId = _contact.Id,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@gmail.com",
            UserId = "New_UserId",
            Salutation = "Ms."
        };

        // act
        _customerRepository.Update(updatedCustomer);
        var customer = _customerRepository.GetById(_contact.Id);

        // assert
        Assert.That(customer.FirstName, Is.EqualTo(updatedCustomer.FirstName));
        Assert.That(customer.LastName, Is.EqualTo(updatedCustomer.LastName));
        Assert.That(customer.Salutation, Is.EqualTo(updatedCustomer.Salutation));

        // These should not be updated
        Assert.That(customer.Email, Is.Not.EqualTo(updatedCustomer.Email));
        Assert.That(customer.UserId, Is.Not.EqualTo(updatedCustomer.UserId));
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        var contactToDel = _context.ContactSet.SingleOrDefault(contact => contact.Id == _contact.Id);
        _context.DeleteObject(contactToDel);
        _context.SaveChanges();

        _context.Dispose();
    }
}