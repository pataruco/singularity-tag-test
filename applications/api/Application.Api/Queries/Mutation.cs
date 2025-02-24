using Application.Api.Helpers;
using Application.Api.Inputs;
using Application.Api.Types;
using Application.Core.Commands;
using Application.Core.Commands.Customer;
using Application.Core.Services.Interfaces;
using Application.Domain.Entities;

namespace Application.Api.Queries;

public class Mutation
{
    public CustomerType? UpdateOneCustomer(
        CustomerWhereUniqueInput where,
        CustomerInput input,
        [Service] ICommandHandler<UpdateCustomerCommand> updateCustomerCommandHandler,
        [Service] ICustomerService customerService)
    {
        var command = new UpdateCustomerCommand(where.ToCustomerId(), input.Salutation);

        updateCustomerCommandHandler.Handle(command);

        Customer? customer = CustomerHelper.GetCustomerById(where, customerService);

        return customer != null ? new CustomerType(customer) : null;
    }
}