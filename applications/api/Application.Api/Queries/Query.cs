using Application.Api.Enums;
using Application.Api.Exceptions;
using Application.Api.Helpers;
using Application.Api.Inputs;
using Application.Api.Types;
using Application.Core.Services.Interfaces;
using Application.Domain.Entities;

namespace Application.Api.Queries;

public class Query
{
    public CustomerType? GetCustomer(CustomerWhereUniqueInput where, [Service] ICustomerService customerService)
    {
        Customer? result = CustomerHelper.GetCustomerById(where, customerService);

        return result == null ? null : new CustomerType(result);

    }
}