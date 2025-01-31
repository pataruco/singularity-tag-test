using Application.Api.Queries;
using Application.Core.Services.Interfaces;
using Application.Core.Services;
using Application.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddDbContext<UserDbContext>(opt => opt.UseInMemoryDatabase("User"))
    .AddScoped<IUserService, UserService>()
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();
    
var app = builder.Build();

app.UseRouting();
app.MapGraphQL();

app.Run();
