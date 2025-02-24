using Application.Api.Filters;
using Application.Api.Queries;
using Application.Core.Services;
using Application.Core.Services.Interfaces;
using Application.Infrastructure.Interfaces;
using Application.Infrastructure.Repositories;
using Application.Infrastructure.Transformers;
using Libraries.Dynamics.DynamicsClient.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Makes configuration from .env file available for the ConfigurationBuilder
if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
    builder.Configuration.AddEnvironmentVariables();
}

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