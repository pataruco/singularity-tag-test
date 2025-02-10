using Application.Api.Enums;
using Application.Api.Inputs;
using Application.Api.Types;
using Application.Core.Services.Interfaces;
using Application.Domain.Entities;

namespace Application.Api.Queries;

public class Query
{
    public User? GetUser(int id, [Service] IUserService userService)
    {
        return userService.GetUser(id);
    }

    public IQueryable<User> GetUsers([Service] IUserService userService)
    {
        return userService.GetUsers();
    }

    public CustomerType? GetCustomer(CustomerWhereUniqueInput where, [Service] ICustomerService customerService)
    {
        var idToQuery = where.GetIdToQuery();

        Customer? result = null;

        switch (idToQuery.Type)
        {
            case CustomerIdType.GraphQLNode:
            case CustomerIdType.User:
                result = customerService.GetByUserId(idToQuery.Value);
                break;
            case CustomerIdType.Contact:
                result = customerService.GetByContactId(idToQuery.Value);
                break;
        }

        if (result == null)
        {
            return null;
        }
        return new CustomerType(result);
    }
}