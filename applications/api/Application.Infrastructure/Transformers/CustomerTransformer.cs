using Application.Domain.Entities;
using Libraries.Dynamics.DynamicsClient;

namespace Application.Infrastructure.Transformers
{
    public class CustomerTransformer
    {
        public static Customer FromContact(Contact contact)
        {
            return new Customer
            {
                Salutation = contact.Salutation,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                ContactId = contact.Id,
                Email = contact.EmailAddress1,
                UserId = contact.New_Auth0Id
            };
        }

        public static Contact MergeCustomerToMutableContact(Contact contact, Customer customer)
        {
            if (customer.Salutation != null)
            {
                contact.Salutation = customer.Salutation;
            }

            if (customer.FirstName != null)
            {
                contact.FirstName = customer.FirstName;
            }

            if (customer.LastName != null)
            {
                contact.LastName = customer.LastName;
            }

            return contact;
        }
    }
}