using FluentValidation;
using JustDirect.Applications.Core.Services.Interfaces;

namespace JustDirect.Applications.Core.Commands.Customer
{
    public class UpdateCustomerCommandHandler(ICustomerService customerService, IValidator<UpdateCustomerCommand> validator)
        : ICommandHandler<UpdateCustomerCommand>
    {
        public void Handle(UpdateCustomerCommand command)
        {
            // TODO: Add logging, authorisation checks, etc.
            var validationResult = validator.Validate(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            customerService.UpdateCustomer(command.Id, command.Salutation);
        }
    }
}