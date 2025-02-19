using Application.Api.Enums;
using Application.Api.Exceptions;
using Application.Api.Inputs;
using Application.Api.Types;
using Application.Core.Services.Interfaces;
using Application.Domain.Entities;

namespace Application.Api.Queries;

public class Query
{
    public CustomerType? GetCustomer(CustomerWhereUniqueInput where, [Service] ICustomerService customerService)
    {
        var idToQuery = where.GetIdToQuery();

        Customer? result = null;

        switch (idToQuery.Type)
        {
            case CustomerIdType.User:
                result = customerService.GetByUserId(idToQuery.Value);
                break;
            case CustomerIdType.GraphQLNode:
            case CustomerIdType.Contact:
                Guid.TryParse(idToQuery.Value, out Guid id);
                if (id == Guid.Empty)
                {
                    throw new InvalidInputException("Invalid GUID provided.");
                }
                result = customerService.GetByContactId(id);
                break;
        }

        if (result == null)
        {
            return null;
        }
        return new CustomerType(result);
    }
}