using Application.Domain.Entities;
using Application.Infrastructure.Interfaces;

namespace Application.Infrastructure.Repositories;

public class CustomerRepository : IRepository<Customer>
{
    List<Customer> _customers;

    public CustomerRepository()
    {
        _customers = new List<Customer>();
        _customers.Add(new Customer()
        {
            UserId = "1",
            ContactId = "1.1",
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@gmail.com",
        });
        _customers.Add(new Customer()
        {
            UserId = "2",
            ContactId = "2.1",
            FirstName = "John",
            LastName = "Foe",
            Email = "john.foe@gmail.com",
        });
        _customers.Add(new Customer()
        {
            UserId = "3",
            ContactId = "3.1",
            FirstName = "John",
            LastName = "Moe",
            Email = "john.moe@gmail.com",
        });
    }
    public IList<Customer> Get()
    {
        return _customers;
    }

    public Customer? GetById(string id)
    {
        return _customers.FirstOrDefault(entity => entity.UserId == id);
    }
}