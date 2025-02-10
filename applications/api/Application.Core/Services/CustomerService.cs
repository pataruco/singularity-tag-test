using Application.Core.Services.Interfaces;
using Application.Domain.Entities;
using Application.Infrastructure.Interfaces;

namespace Application.Core.Services;

public class CustomerService(IRepository<Customer> customerRepository) : ICustomerService
{
    public Customer? GetByUserId(string id)
    {
        return customerRepository.GetById(id);
    }

    public Customer? GetByContactId(string id)
    {
        return customerRepository.Get().FirstOrDefault(x => x.ContactId == id);
    }
}