using Application.Domain.Entities;
using Application.Core.Services.Interfaces;

namespace Application.Api.Queries;

public class Mutation
{
    public async Task<User> CreateUser(UserDTO user, [Service] IUserService userService)
    {
        return await userService.CreateUser(user);
    }

    public async Task<User> UpdateUser(int id, UserDTO user, [Service] IUserService userService)
    {
        return await userService.UpdateUser(id, user);
    }

    public async Task<int> DeleteUser(int id,[Service] IUserService userService)
    {
        return await userService.DeleteUser(id);
    }
}
