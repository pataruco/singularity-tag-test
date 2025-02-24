using Application.Api.Filters;
using Application.Api.Queries;
using Application.Core.Commands;
using Application.Core.Commands.Customer;
using Application.Core.Services;
using Application.Core.Services.Interfaces;
using Application.Infrastructure.Interfaces;
using Application.Infrastructure.Repositories;
using Application.Infrastructure.Transformers;
using FluentValidation;
using Libraries.Dynamics.DynamicsClient.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Makes configuration from .env file available for the ConfigurationBuilder
if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
    builder.Configuration.AddEnvironmentVariables();
}

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
builder
    .Services
    .AddScoped<ICustomerRepository, CustomerRepository>()
    .AddScoped<ICustomerService, CustomerService>()
    .AddScoped<ICommandHandler<UpdateCustomerCommand>, UpdateCustomerCommandHandler>()
    .AddScoped<IValidator<UpdateCustomerCommand>, UpdateCustomerCommandValidator>()
    .AddDynamicsClient(configuration)
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddGlobalObjectIdentification()
    .AddErrorFilter<CustomErrorFilter>();

var app = builder.Build();

app.UseRouting();
app.MapGraphQL();

app.Run();