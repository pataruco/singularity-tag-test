using Application.Core.Services.Interfaces;
using Application.Domain.Entities;

namespace Application.Api.Queries;

public class Query
{
    public User? GetUser(int id, [Service] IUserService userService)
    {
        return userService.GetUser(id);
    }

    public IQueryable<User> GetUsers([Service] IUserService userService)
    {
        return userService.GetUsers();
    }
}