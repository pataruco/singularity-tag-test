using JustDirect.Applications.Domain.Entities;
using JustDirect.Applications.Domain.Models;

namespace JustDirect.Applications.Core.Services.Interfaces;

public interface ICustomerService
{
    public Customer? Get(CustomerId id);

    public Customer? GetByContactId(Guid id);

    public Customer? GetByUserId(string id);

    void UpdateCustomer(CustomerId id, string? salutation);
}