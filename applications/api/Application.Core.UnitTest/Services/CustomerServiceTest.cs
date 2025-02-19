using Application.Core.Services;
using Application.Core.Services.Interfaces;
using Application.Domain.Entities;
using Application.Infrastructure.Interfaces;
using Moq;

namespace Application.Core.UnitTest.Services
{
    public class CustomerServiceTest
    {
        Mock<ICustomerRepository> _mockCustomerRepository;
        ICustomerService _customerService;
        Customer _customer;

        [SetUp]
        public void Setup()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_mockCustomerRepository.Object);
            _customer = new Customer()
            {
                ContactId = Guid.NewGuid(),
                Email = "test@test.com",
            };
        }

        [Test]
        public void GetByContactId_ShouldCallRepositoryWithContactId()
        {
            // arrange
            _mockCustomerRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(_customer);

            // act
            _customerService.GetByContactId(_customer.ContactId);

            // assert
            _mockCustomerRepository.Verify(repo => repo.GetById(It.Is<Guid>(val => val == _customer.ContactId)), Times.Once);
        }

        [Test]
        public void GetByContactId_ShouldReturnCustomerWhenRepositoryReturnsCustomer()
        {
            // arrange
            _mockCustomerRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(_customer);

            // act
            var result = _customerService.GetByContactId(_customer.ContactId);

            // assert
            Assert.That(result, Is.EqualTo(_customer));
        }

        [Test]
        public void GetByContactId_ShouldReturnNullWhenRepositoryReturnsNull()
        {
            // arrange
            _mockCustomerRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns((Customer)null);

            // act
            var result = _customerService.GetByContactId(_customer.ContactId);

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
            _mockCustomerRepository.Verify(repo => repo.GetByAuth0Id(It.Is<string>(val => val == _customer.UserId)), Times.Once);
        }

        [Test]
        public void GetByUserId_ShouldReturnCustomerWhenRepositoryReturnsCustomer()
        {
            // arrange
            _mockCustomerRepository.Setup(repo => repo.GetByAuth0Id(It.IsAny<string>())).Returns(_customer);

            // act
            var result = _customerService.GetByUserId(_customer.UserId);

            // assert
            Assert.That(result, Is.EqualTo(_customer));
        }

        [Test]
        public void GetByUserId_ShouldReturnNullWhenRepositoryReturnsNull()
        {
            // arrange
            _mockCustomerRepository.Setup(repo => repo.GetByAuth0Id(It.IsAny<string>())).Returns((Customer)null);

            // act
            var result = _customerService.GetByUserId(_customer.UserId);

            // assert
            Assert.That(result, Is.EqualTo(null));
        }

    }
}