using Application.Domain.Entities;
using Libraries.Dynamics.DynamicsClient;

namespace Application.Infrastructure.Transformers
{
    public class CustomerTransformer
    {
        public Customer FromContact(Contact contact)
        {
            return new Customer()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                ContactId = contact.Id,
                Email = contact.EmailAddress1,
                UserId = contact.New_Auth0Id
            };
        }
    }
}