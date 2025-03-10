using JustDirect.Applications.Core.Commands;
using JustDirect.Applications.Core.Commands.Customer;
using JustDirect.Applications.Core.Services.Interfaces;
using JustDirect.Applications.CustomerApi.Helpers;
using JustDirect.Applications.CustomerApi.Inputs;
using JustDirect.Applications.CustomerApi.Types;
using JustDirect.Applications.Domain.Entities;

namespace JustDirect.Applications.CustomerApi.Queries;

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