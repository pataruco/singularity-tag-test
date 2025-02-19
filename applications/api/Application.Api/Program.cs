using Application.Api.Filters;
using Application.Api.Queries;
using Application.Core.Services;
using Application.Core.Services.Interfaces;
using Application.Domain.Entities;
using Application.Infrastructure.Interfaces;
using Application.Infrastructure.Repositories;
using Application.Infrastructure.Transformers;
using Libraries.Dynamics.DynamicsClient.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddDynamicsClient(builder.Configuration)
    .AddScoped<CustomerTransformer>()
    .AddScoped<ICustomerRepository, CustomerRepository>()
    .AddScoped<ICustomerService, CustomerService>()
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddGlobalObjectIdentification()
    .AddErrorFilter<CustomErrorFilter>();

var app = builder.Build();

app.UseRouting();
app.MapGraphQL();

app.Run();