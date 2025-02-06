using Application.Domain.Entities;

namespace Application.Core.Services.Interfaces;

public interface IUserService
{
    public User? GetUser(int id);
    public IQueryable<User> GetUsers();
    public Task<User> CreateUser(UserDTO input);
    public Task<User> UpdateUser(int id, UserDTO input);
    public Task<int> DeleteUser(int id);
}