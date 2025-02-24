using Application.Core.Commands.Customer;
using Application.Core.Services.Interfaces;
using Application.Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Application.Core.UnitTest.Commands
{
    [TestFixture]
    public class UpdateCustomerCommandHandlerTests
    {
        private Mock<ICustomerService> _mockCustomerService;
        private Mock<IValidator<UpdateCustomerCommand>> _mockValidator;
        private UpdateCustomerCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _mockValidator = new Mock<IValidator<UpdateCustomerCommand>>();

            _handler = new UpdateCustomerCommandHandler(
                _mockCustomerService.Object,
                _mockValidator.Object
            );
        }

        [Test]
        public void Handle_ShouldCallUpdateCustomer_WhenCommandIsValid()
        {
            // Arrange
            var customerId = new CustomerId { Id = Guid.NewGuid() };
            var command = new UpdateCustomerCommand(customerId, "Dr.");

            _mockValidator
                .Setup(v => v.Validate(command))
                .Returns(new ValidationResult());

            // Act
            _handler.Handle(command);

            // Assert
            _mockCustomerService.Verify(s =>
                    s.UpdateCustomer(customerId, "Dr."),
                Times.Once);

            _mockValidator.Verify(v => v.Validate(command), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowValidationException_WhenCommandIsInvalid()
        {
            // Arrange
            var customerId = new CustomerId { Id = Guid.NewGuid() };
            var command = new UpdateCustomerCommand(customerId, "InvalidSalutation");

            var failures = new[] { new ValidationFailure(nameof(command.Salutation), "Invalid salutation.") };

            _mockValidator
                .Setup(v => v.Validate(command))
                .Returns(new ValidationResult(failures));

            // Act & Assert
            var ex = Assert.Throws<ValidationException>(() => _handler.Handle(command));

            Assert.That(ex!.Errors, Is.EqualTo(failures));
            _mockCustomerService.Verify(s => s.UpdateCustomer(It.IsAny<CustomerId>(), It.IsAny<string>()), Times.Never);
            _mockValidator.Verify(v => v.Validate(command), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenCustomerServiceUpdateFails()
        {
            // Arrange
            var customerId = new CustomerId { Id = Guid.NewGuid() };
            var command = new UpdateCustomerCommand(customerId, "Dr.");

            _mockValidator
                .Setup(v => v.Validate(command))
                .Returns(new ValidationResult());

            _mockCustomerService
                .Setup(s => s.UpdateCustomer(customerId, "Dr."))
                .Throws(new Exception("Update failed"));

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _handler.Handle(command));

            Assert.That(ex!.Message, Is.EqualTo("Update failed"));
            _mockValidator.Verify(v => v.Validate(command), Times.Once);
            _mockCustomerService.Verify(s => s.UpdateCustomer(customerId, "Dr."), Times.Once);
        }
    }
}