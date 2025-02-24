using Application.Api.Enums;
using Application.Api.Exceptions;
using Application.Api.Inputs;
using Application.Api.Structs;
using Application.Core.Services.Interfaces;
using Application.Domain.Entities;
using Application.Domain.Models;

namespace Application.Api.Helpers;

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