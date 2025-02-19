using Application.Core.Services.Interfaces;
using Application.Domain.Entities;
using Application.Infrastructure.Interfaces;

namespace Application.Core.Services;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public Customer? GetByUserId(string id)
    {
        return customerRepository.GetByAuth0Id(id);
    }

    public Customer? GetByContactId(Guid id)
    {
        return customerRepository.GetById(id);
    }
}