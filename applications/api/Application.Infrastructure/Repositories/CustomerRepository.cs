using Application.Domain.Entities;
using Application.Infrastructure.Interfaces;
using Application.Infrastructure.Transformers;
using Libraries.Dynamics.DynamicsClient;
using Libraries.Dynamics.DynamicsClient.Factories;

namespace Application.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly DataverseContext _context;
    private readonly CustomerTransformer _customerTransformer;
    public CustomerRepository(IDataverseContextFactory dataverseContextFactory, CustomerTransformer customerCustomerTransformer)
    {
        _context = dataverseContextFactory.CreateDataverseContext();
        _customerTransformer = customerCustomerTransformer;
    }
    public IList<Customer> Get()
    {
        throw new NotImplementedException();
    }

    public Customer? GetByAuth0Id(string id)
    {
        Contact? contact = _context.ContactSet.SingleOrDefault(entity => entity.New_Auth0Id == id);
        if (contact == null)
        {
            return null;
        }

        return _customerTransformer.FromContact(contact);
    }

    public Customer? GetById(Guid id)
    {
        Contact? contact = _context.ContactSet.SingleOrDefault(entity => entity.Id == id);
        if (contact == null)
        {
            return null;
        }

        return _customerTransformer.FromContact(contact);
    }
}