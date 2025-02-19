using Application.Domain.Entities;

namespace Application.Infrastructure.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer? GetByAuth0Id(string id);
    }
}