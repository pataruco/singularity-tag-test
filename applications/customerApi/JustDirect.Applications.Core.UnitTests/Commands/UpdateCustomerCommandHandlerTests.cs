using FluentValidation;
using FluentValidation.Results;
using JustDirect.Applications.Core.Commands.Customer;
using JustDirect.Applications.Core.Services.Interfaces;
using JustDirect.Applications.Domain.Models;
using Moq;

namespace JustDirect.Applications.Core.UnitTests.Commands
{
    [TestFixture]
    public class UpdateCustomerCommandHandlerTests
    {
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

        private Mock<ICustomerService> _mockCustomerService;
        private Mock<IValidator<UpdateCustomerCommand>> _mockValidator;
        private UpdateCustomerCommandHandler _handler;

        [Test]
        public void Handle_ShouldCallUpdateCustomer_WhenCommandIsValid()
        {
            // Arrange
            CustomerId customerId = new() { Id = Guid.NewGuid() };
            UpdateCustomerCommand command = new(customerId, "Dr.");

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
            CustomerId customerId = new() { Id = Guid.NewGuid() };
            UpdateCustomerCommand command = new(customerId, "InvalidSalutation");

            ValidationFailure[] failures =
                new[] { new ValidationFailure(nameof(command.Salutation), "Invalid salutation.") };

            _mockValidator
                .Setup(v => v.Validate(command))
                .Returns(new ValidationResult(failures));

            // Act & Assert
            ValidationException? ex = Assert.Throws<ValidationException>(() => _handler.Handle(command));

            Assert.That(ex!.Errors, Is.EqualTo(failures));
            _mockCustomerService.Verify(s => s.UpdateCustomer(It.IsAny<CustomerId>(), It.IsAny<string>()), Times.Never);
            _mockValidator.Verify(v => v.Validate(command), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowException_WhenCustomerServiceUpdateFails()
        {
            // Arrange
            CustomerId customerId = new() { Id = Guid.NewGuid() };
            UpdateCustomerCommand command = new(customerId, "Dr.");

            _mockValidator
                .Setup(v => v.Validate(command))
                .Returns(new ValidationResult());

            _mockCustomerService
                .Setup(s => s.UpdateCustomer(customerId, "Dr."))
                .Throws(new Exception("Update failed"));

            // Act & Assert
            Exception? ex = Assert.Throws<Exception>(() => _handler.Handle(command));

            Assert.That(ex!.Message, Is.EqualTo("Update failed"));
            _mockValidator.Verify(v => v.Validate(command), Times.Once);
            _mockCustomerService.Verify(s => s.UpdateCustomer(customerId, "Dr."), Times.Once);
        }
    }
}