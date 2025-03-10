using JustDirect.Applications.CustomerApi.Types;
using JustDirect.Applications.Domain.Entities;

namespace JustDirect.Applications.CustomerApi.IntegrationTests.Types
{
    public class CustomerTypeTests
    {
        [Test]
        public void CustomerType_PopulatesFieldsCorrectlyFromCustomerObject()
        {
            // arrange
            Customer customer = new()
            {
                UserId = Guid.NewGuid().ToString(),
                ContactId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com"
            };

            // act
            CustomerType res = new(customer);

            // assert
            Assert.That(res.Id, Is.EqualTo(customer.ContactId.ToString()));
            Assert.That(res.UserId, Is.EqualTo(customer.UserId));
            Assert.That(res.ContactId, Is.EqualTo(customer.ContactId.ToString()));
            Assert.That(res.FirstName, Is.EqualTo(customer.FirstName));
            Assert.That(res.LastName, Is.EqualTo(customer.LastName));
            Assert.That(res.Email, Is.EqualTo(customer.Email));
        }

        [Test]
        public void CustomerType_PopulatesFieldsCorrectlyFromCustomerObjectIncludingNulls()
        {
            // arrange
            Customer customer = new()
            {
                UserId = null,
                ContactId = Guid.NewGuid(),
                FirstName = null,
                LastName = null,
                Email = "john.doe@gmail.com"
            };

            // act
            CustomerType res = new(customer);

            // assert
            Assert.That(res.Id, Is.EqualTo(customer.ContactId.ToString()));
            Assert.That(res.UserId, Is.Null);
            Assert.That(res.ContactId, Is.EqualTo(customer.ContactId.ToString()));
            Assert.That(res.FirstName, Is.Null);
            Assert.That(res.LastName, Is.Null);
            Assert.That(res.Email, Is.EqualTo(customer.Email));
        }
    }
}