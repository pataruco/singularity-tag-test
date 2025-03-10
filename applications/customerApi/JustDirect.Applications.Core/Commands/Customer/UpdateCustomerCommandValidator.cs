using FluentValidation;

namespace JustDirect.Applications.Core.Commands.Customer
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(cmd => cmd.Id).NotNull();
            RuleFor(cmd => cmd.Salutation).MaximumLength(100);
        }
    }
}