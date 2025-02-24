using Application.Core.Commands.Customer;
using Application.Domain.Models;
using FluentValidation.TestHelper;

namespace Application.Core.UnitTest.Commands
{
    public class UpdateCustomerCommandValidatorTests
    {
        private UpdateCustomerCommandValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new UpdateCustomerCommandValidator();
        }

        [Test]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            // Arrange
            var customerId = new CustomerId { Id = Guid.NewGuid() };
            var command = new UpdateCustomerCommand(customerId, "Dr.");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Validate_ShouldFail_WhenIdIsNull()
        {
            // Arrange
            var command = new UpdateCustomerCommand(null!, "Dr.");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.Id)
                .WithErrorMessage("'Id' must not be empty.");
        }

        [Test]
        public void Validate_ShouldFail_WhenSalutationExceedsMaxLength()
        {
            // Arrange
            var customerId = new CustomerId { Id = Guid.NewGuid() };
            var longSalutation = new string('A', 101); // 101 characters
            var command = new UpdateCustomerCommand(customerId, longSalutation);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.Salutation)
                .WithErrorMessage("The length of 'Salutation' must be 100 characters or fewer. You entered 101 characters.");
        }

        [Test]
        public void Validate_ShouldPass_WhenSalutationIsNull()
        {
            // Arrange
            var customerId = new CustomerId { Id = Guid.NewGuid() };
            var command = new UpdateCustomerCommand(customerId, null);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}