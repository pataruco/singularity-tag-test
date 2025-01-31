using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Infrastructure.Data;
public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}