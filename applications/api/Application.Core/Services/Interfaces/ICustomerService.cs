using Application.Domain.Entities;
using Application.Domain.Models;

namespace Application.Core.Services.Interfaces;

public interface ICustomerService
{
    public Customer? Get(CustomerId id);

    public Customer? GetByContactId(Guid id);

    public Customer? GetByUserId(string id);

    void UpdateCustomer(CustomerId id, string? salutation);
}