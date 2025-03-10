using JustDirect.Applications.Domain.Entities;

namespace JustDirect.Applications.Infrastructure.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer? GetByAuth0Id(string id);
    }
}