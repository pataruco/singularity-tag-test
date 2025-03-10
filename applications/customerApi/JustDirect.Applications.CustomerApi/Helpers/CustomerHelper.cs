using JustDirect.Applications.Core.Services.Interfaces;
using JustDirect.Applications.CustomerApi.Enums;
using JustDirect.Applications.CustomerApi.Exceptions;
using JustDirect.Applications.CustomerApi.Inputs;
using JustDirect.Applications.CustomerApi.Structs;
using JustDirect.Applications.Domain.Entities;
using JustDirect.Applications.Domain.Models;

namespace JustDirect.Applications.CustomerApi.Helpers;

public static class CustomerHelper
{
    public static Customer? GetCustomerById(CustomerWhereUniqueInput where, ICustomerService customerService)
    {
        CustomerIdStruct idToQuery = where.GetIdToQuery();

        Customer? customer = idToQuery.Type switch
        {
            CustomerIdType.User => customerService.GetByUserId(idToQuery.Value),
            CustomerIdType.GraphQLNode or CustomerIdType.Contact => GetCustomerByContactId(idToQuery.Value,
                customerService),
            _ => null
        };

        return customer;
    }

    private static Customer? GetCustomerByContactId(string value, ICustomerService customerService)
    {
        if (!Guid.TryParse(value, out Guid contactId))
        {
            throw new InvalidInputException("Invalid GUID provided.");
        }

        return customerService.GetByContactId(contactId);
    }

    public static CustomerId MapToCustomerId(CustomerWhereUniqueInput input)
    {
        Guid? id = null;
        if (!string.IsNullOrEmpty(input.Id))
        {
            if (!Guid.TryParse(input.Id, out Guid parsedId))
            {
                throw new InvalidInputException($"Invalid GUID provided for {nameof(input.Id)}.");
            }
            id = parsedId;
        }

        Guid? contactId = null;
        if (!string.IsNullOrEmpty(input.ContactId))
        {
            if (!Guid.TryParse(input.ContactId, out Guid parsedContactId))
            {
                throw new InvalidInputException($"Invalid GUID provided for {nameof(input.ContactId)}.");
            }
            contactId = parsedContactId;
        }

        return new CustomerId
        {
            Id = id,
            UserId = input.UserId,
            ContactId = contactId
        };
    }
}