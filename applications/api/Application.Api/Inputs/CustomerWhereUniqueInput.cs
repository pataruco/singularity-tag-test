using Application.Api.Enums;
using Application.Api.Exceptions;
using Application.Api.Helpers;
using Application.Api.Structs;
using Application.Domain.Entities;
using Application.Domain.Models;

namespace Application.Api.Inputs;

public class CustomerWhereUniqueInput
{
    [ID(nameof(Customer))]
    public string? Id { get; set; }

    [GraphQLType(typeof(IdType))]
    public string? UserId { get; set; }

    [GraphQLType(typeof(IdType))]
    public string? ContactId { get; set; }

    public CustomerId ToCustomerId()
    {
        // Piggybacking off the existing validation logic in GetIdToQuery()
        _ = this.GetIdToQuery();
        return CustomerHelper.MapToCustomerId(this);
    }

    public CustomerIdStruct GetIdToQuery()
    {
        CustomerIdStruct? idToQuery = null;

        if (!string.IsNullOrEmpty(Id))
        {
            idToQuery = new CustomerIdStruct()
            {
                Value = Id,
                Type = CustomerIdType.GraphQLNode
            };
        }

        if (!string.IsNullOrEmpty(UserId))
        {
            if (idToQuery != null)
            {
                throw new InvalidInputException("Only one type of ID can be specified in input.");
            }
            idToQuery = new CustomerIdStruct()
            {
                Value = UserId,
                Type = CustomerIdType.User
            };
        }

        if (!string.IsNullOrEmpty(ContactId))
        {
            if (idToQuery != null)
            {
                throw new InvalidInputException("Only one type of ID can be specified in input.");
            }
            idToQuery = new CustomerIdStruct()
            {
                Value = ContactId,
                Type = CustomerIdType.Contact
            };
        }

        return idToQuery ?? throw new InvalidInputException("One type of ID must be provided in input.");
    }
}