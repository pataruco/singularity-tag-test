using Application.Domain.Entities;
using Application.Infrastructure.Interfaces;
using Application.Infrastructure.Transformers;
using Libraries.Dynamics.DynamicsClient;
using Libraries.Dynamics.DynamicsClient.Factories;

namespace Application.Infrastructure.Repositories;

public class CustomerRepository(IDataverseContextFactory dataverseContextFactory) : ICustomerRepository
{
    private readonly DataverseContext _context = dataverseContextFactory.CreateDataverseContext();

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