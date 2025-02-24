using Application.Api.Exceptions;
using Application.Core.Services.Interfaces;
using Application.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Application.Api.Types;

[Node]
[GraphQLName("Customer")]
public class CustomerType
{
    public required string Id { get; set; }
    [GraphQLType(typeof(IdType))]
    public string? UserId { get; set; }
    [GraphQLNonNullType]
    [GraphQLType(typeof(IdType))]
    public required string ContactId { get; set; }
    public required string Email { get; set; }
    public string? Salutation { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [SetsRequiredMembers]
    public CustomerType(Customer customer)
    {
        Id = customer.ContactId.ToString();
        UserId = customer.UserId;
        ContactId = customer.ContactId.ToString();
        Email = customer.Email;
        Salutation = customer.Salutation;
        FirstName = customer.FirstName;
        LastName = customer.LastName;
    }

    public static CustomerType? Get(string id, ICustomerService customerService)
    {
        Guid.TryParse(id, out Guid validId);
        if (validId == Guid.Empty)
        {
            throw new InvalidInputException("Invalid GUID provided.");
        }

        Customer? customer = customerService.GetByContactId(validId);

        if (customer == null)
        {
            return null;
        }

        return new CustomerType(customer);
    }
}