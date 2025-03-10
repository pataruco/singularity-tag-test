using JustDirect.Applications.Domain.Entities;
using JustDirect.Applications.Infrastructure.Interfaces;
using JustDirect.Applications.Infrastructure.Transformers;
using Libraries.Dynamics.DynamicsClient;
using Libraries.Dynamics.DynamicsClient.Factories;

namespace JustDirect.Applications.Infrastructure.Repositories;

public class CustomerRepository(IContextFactory<DataverseContext> contextFactory) : ICustomerRepository
{
    private readonly DataverseContext _context = contextFactory.CreateContext();

    public IList<Customer> Get()
    {
        throw new NotImplementedException();
    }

    public Customer? GetByAuth0Id(string id)
    {
        Contact? contact = _context.ContactSet.SingleOrDefault(entity => entity.universe_Auth0ID == id);
        if (contact == null)
        {
            return null;
        }

        return CustomerTransformer.FromContact(contact);
    }

    public Customer? GetById(Guid id)
    {
        Contact? contact = _context.ContactSet.SingleOrDefault(entity => entity.Id == id);
        if (contact == null)
        {
            return null;
        }

        return CustomerTransformer.FromContact(contact);
    }

    public void Update(Customer entity)
    {
        Contact? contact = _context.ContactSet.SingleOrDefault(c => c.ContactId == entity.ContactId);
        if (contact == null)
        {
            return;
        }

        // TODO: Do we need to worry about race conditions here, i.e. changes made directly in CRM?
        contact = CustomerTransformer.MergeCustomerToMutableContact(contact, entity);

        _context.UpdateObject(contact);
        _context.SaveChanges();
    }
}