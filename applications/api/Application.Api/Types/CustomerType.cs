using Application.Core.Services.Interfaces;
using Application.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Application.Api.Types;

[Node]
[GraphQLName("Customer")]
public class CustomerType
{
    public required string Id { get; set; }
    [GraphQLNonNullType]
    [GraphQLType(typeof(IdType))]
    public required string UserId { get; set; }
    [GraphQLNonNullType]
    [GraphQLType(typeof(IdType))]
    public required string ContactId { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [SetsRequiredMembers]
    public CustomerType(Customer customer)
    {
        Id = customer.UserId;
        UserId = customer.UserId;
        ContactId = customer.ContactId;
        Email = customer.Email;
        FirstName = customer.FirstName;
        LastName = customer.LastName;
    }

    public static CustomerType? Get(string id, ICustomerService customerService)
    {
        Customer? customer = customerService.GetByUserId(id);

        if (customer == null)
        {
            return null;
        }

        return new CustomerType(customer);
    }
}