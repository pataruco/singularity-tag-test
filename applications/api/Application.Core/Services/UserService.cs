
using Application.Domain.Entities;
using Application.Core.Services.Interfaces;
using Application.Infrastructure.Data;

namespace Application.Core.Services;
public class UserService(UserDbContext context) : IUserService
{
    public User? GetUser(int id)
    {
        return context.Users.Find(id);
    }

    public IQueryable<User> GetUsers()
    {
        return context.Users;
    }

    public async Task<User> CreateUser(UserDTO input)
    {
        var user = new User()
        {
            Name = input.Name,
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }
    
    public async Task<User> UpdateUser(int id, UserDTO input)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null)
        {
            throw new ArgumentException($"User with ID {id} not found.");
        }

        user.Name = input.Name;
        await context.SaveChangesAsync();

        return user;
    }
    
    public async Task<int> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null)
        {
            throw new ArgumentException($"User with ID {id} not found.");
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return user.Id;
    }
}