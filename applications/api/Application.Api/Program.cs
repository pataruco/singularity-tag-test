using Application.Api.Filters;
using Application.Api.Queries;
using Application.Core.Services;
using Application.Core.Services.Interfaces;
using Application.Domain.Entities;
using Application.Infrastructure.Data;
using Application.Infrastructure.Interfaces;
using Application.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddDbContext<UserDbContext>(opt => opt.UseInMemoryDatabase("User"))
    .AddSingleton<IRepository<Customer>, CustomerRepository>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<ICustomerService, CustomerService>()
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddGlobalObjectIdentification()
    .AddErrorFilter<CustomErrorFilter>();

var app = builder.Build();

app.UseRouting();
app.MapGraphQL();

app.Run();