using Application.Domain.Models;

namespace Application.Core.Commands.Customer
{
    public class UpdateCustomerCommand(CustomerId id, string? salutation)
    {
        public CustomerId Id { get; } = id;

        public string? Salutation { get; } = salutation;
    }
}