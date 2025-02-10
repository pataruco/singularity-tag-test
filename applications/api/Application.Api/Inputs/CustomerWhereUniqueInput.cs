using Application.Api.Enums;
using Application.Api.Exceptions;
using Application.Api.Structs;

namespace Application.Api.Inputs;

public class CustomerWhereUniqueInput
{
    [ID]
    public string? Id { get; set; }

    [GraphQLType(typeof(IdType))]
    public string? UserId { get; set; }

    [GraphQLType(typeof(IdType))]
    public string? ContactId { get; set; }

    public CustomerIdStruct GetIdToQuery()
    {
        CustomerIdStruct? idToQuery = null;

        if (!String.IsNullOrEmpty(Id))
        {
            idToQuery = new CustomerIdStruct()
            {
                Value = Id,
                Type = CustomerIdType.GraphQLNode
            };
        }

        if (!String.IsNullOrEmpty(UserId))
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

        if (!String.IsNullOrEmpty(ContactId))
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