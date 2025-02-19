using Application.Domain.Entities;

namespace Application.Core.Services.Interfaces;

public interface ICustomerService
{
    public Customer? GetByContactId(Guid id);

    public Customer? GetByUserId(string id);

}