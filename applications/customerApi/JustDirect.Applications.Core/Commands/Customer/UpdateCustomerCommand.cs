using JustDirect.Applications.Domain.Models;

namespace JustDirect.Applications.Core.Commands.Customer
{
    public class UpdateCustomerCommand(CustomerId id, string? salutation)
    {
        public CustomerId Id { get; } = id;

        public string? Salutation { get; } = salutation;
    }
}