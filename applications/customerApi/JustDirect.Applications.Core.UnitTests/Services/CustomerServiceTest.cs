using JustDirect.Applications.Core.Services;
using JustDirect.Applications.Core.Services.Interfaces;
using JustDirect.Applications.Domain.Entities;
using JustDirect.Applications.Domain.Models;
using JustDirect.Applications.Infrastructure.Interfaces;
using Moq;

namespace JustDirect.Applications.Core.UnitTests.Services
{
    public class CustomerServiceTest
    {
        private Guid _contactId;
        private Customer _customer;
        private ICustomerService _customerService;
        private Mock<ICustomerRepository> _mockCustomerRepository;
        private string _userId;

        [SetUp]
        public void Setup()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_mockCustomerRepository.Object);
            _contactId = Guid.NewGuid();
            _userId = "idp|123456";

            _customer = new Customer { ContactId = _contactId, UserId = _userId, Email = "test@test.com" };
        }

        [Test]
        public void GetByContactId_ShouldCallRepositoryWithContactId()
        {
            // arrange
            _mockCustomerRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(_customer);

            // act
            _customerService.GetByContactId(_customer.ContactId);

            // assert
            _mockCustomerRepository.Verify(repo => repo.GetById(It.Is<Guid>(val => val == _customer.ContactId)),
                Times.Once);
        }

        [Test]
        public void GetByContactId_ShouldReturnCustomerWhenRepositoryReturnsCustomer()
        {
            // arrange
            _mockCustomerRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(_customer);

            // act
            Customer? result = _customerService.GetByContactId(_customer.ContactId);

            // assert
            Assert.That(result, Is.EqualTo(_customer));
        }

        [Test]
        public void GetByContactId_ShouldReturnNullWhenRepositoryReturnsNull()
        {
            // arrange
            _mockCustomerRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns((Customer)null);

            // act
            Customer? result = _customerService.GetByContactId(_customer.ContactId);

            // assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void GetByUserId_ShouldCallRepositoryWithUserId()
        {
            // arrange
            _mockCustomerRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(_customer);

            // act
            _customerService.GetByUserId(_customer.UserId);

            // assert
            _mockCustomerRepository.Verify(repo => repo.GetByAuth0Id(It.Is<string>(val => val == _customer.UserId)),
                Times.Once);
        }

        [Test]
        public void GetByUserId_ShouldReturnCustomerWhenRepositoryReturnsCustomer()
        {
            // arrange
            _mockCustomerRepository.Setup(repo => repo.GetByAuth0Id(It.IsAny<string>())).Returns(_customer);

            // act
            Customer? result = _customerService.GetByUserId(_customer.UserId);

            // assert
            Assert.That(result, Is.EqualTo(_customer));
        }

        [Test]
        public void GetByUserId_ShouldReturnNullWhenRepositoryReturnsNull()
        {
            // arrange
            _mockCustomerRepository.Setup(repo => repo.GetByAuth0Id(It.IsAny<string>())).Returns((Customer)null);

            // act
            Customer? result = _customerService.GetByUserId(_customer.UserId);

            // assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void Get_ShouldReturnCustomer_WhenContactIdIsProvided()
        {
            // Arrange
            CustomerId customerId = new() { ContactId = _contactId };
            _mockCustomerRepository.Setup(repo => repo.GetById(_contactId)).Returns(_customer);

            // Act
            Customer? result = _customerService.Get(customerId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(_customer));
            _mockCustomerRepository.Verify(repo => repo.GetById(_contactId), Times.Once);
        }

        [Test]
        public void Get_ShouldReturnCustomer_WhenUserIdIsProvided()
        {
            // Arrange
            CustomerId customerId = new() { UserId = _userId };
            _mockCustomerRepository.Setup(repo => repo.GetByAuth0Id(_userId)).Returns(_customer);

            // Act
            Customer? result = _customerService.Get(customerId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(_customer));
            _mockCustomerRepository.Verify(repo => repo.GetByAuth0Id(_userId), Times.Once);
        }

        [Test]
        public void Get_ShouldReturnCustomer_WhenIdIsProvided()
        {
            // Arrange
            CustomerId customerId = new() { Id = _contactId };
            _mockCustomerRepository.Setup(repo => repo.GetById(_contactId)).Returns(_customer);

            // Act
            Customer? result = _customerService.Get(customerId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(_customer));
            _mockCustomerRepository.Verify(repo => repo.GetById(_contactId), Times.Once);
        }

        [Test]
        public void Get_ShouldThrowInvalidOperationException_WhenInvalidCustomerIdIsProvided()
        {
            // Arrange
            CustomerId customerId = new();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _customerService.Get(customerId));
        }

        [Test]
        public void Get_ShouldReturnNull_WhenCustomerNotFound()
        {
            // Arrange
            CustomerId customerId = new() { ContactId = _contactId };
            _mockCustomerRepository.Setup(repo => repo.GetById(_contactId)).Returns((Customer)null);

            // Act
            Customer? result = _customerService.Get(customerId);

            // Assert
            Assert.That(result, Is.Null);
            _mockCustomerRepository.Verify(repo => repo.GetById(_contactId), Times.Once);
        }
    }
}