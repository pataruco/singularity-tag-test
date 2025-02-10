using Application.Domain.Entities;

namespace Application.Core.Services.Interfaces;

public interface ICustomerService
{
    public Customer? GetByContactId(string id);

    public Customer? GetByUserId(string id);

}