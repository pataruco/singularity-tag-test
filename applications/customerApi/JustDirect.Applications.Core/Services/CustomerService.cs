using JustDirect.Applications.Core.Services.Interfaces;
using JustDirect.Applications.Domain.Entities;
using JustDirect.Applications.Domain.Models;
using JustDirect.Applications.Infrastructure.Interfaces;

namespace JustDirect.Applications.Core.Services;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public Customer? Get(CustomerId id)
    {
        if (id.IsContactId)
        {
            return GetByContactId(id.ContactId.Value);
        }
        else if (id.IsUserId)
        {
            return customerRepository.GetByAuth0Id(id.UserId);
        }
        else if (id.IsId)
        {
            return GetByContactId(id.Id.Value);
        }

        throw new InvalidOperationException("Invalid CustomerId");
    }

    public Customer? GetByUserId(string id)
    {
        return customerRepository.GetByAuth0Id(id);
    }

    public Customer? GetByContactId(Guid id)
    {
        return customerRepository.GetById(id);
    }

    public void UpdateCustomer(CustomerId id, string? salutation)
    {
        var customer = Get(id);
        if (customer == null)
        {
            throw new InvalidOperationException("Customer not found.");
        }

        // We only want to update "mutable" fields, and only if the customer property has a value
        if (salutation != null)
        {
            customer.Salutation = salutation;
        }

        customerRepository.Update(customer);
    }
}