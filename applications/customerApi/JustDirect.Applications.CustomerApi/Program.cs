using FluentValidation;
using JustDirect.Applications.Core.Commands;
using JustDirect.Applications.Core.Commands.Customer;
using JustDirect.Applications.Core.Services;
using JustDirect.Applications.Core.Services.Interfaces;
using JustDirect.Applications.CustomerApi.Filters;
using JustDirect.Applications.CustomerApi.Queries;
using JustDirect.Applications.Infrastructure.Interfaces;
using JustDirect.Applications.Infrastructure.Repositories;
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
    .AddScoped<ICustomerRepository, CustomerRepository>()
    .AddScoped<ICustomerService, CustomerService>()
    .AddScoped<ICommandHandler<UpdateCustomerCommand>, UpdateCustomerCommandHandler>()
    .AddScoped<IValidator<UpdateCustomerCommand>, UpdateCustomerCommandValidator>()
    .AddDynamicsClient(builder.Configuration)
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddGlobalObjectIdentification()
    .AddErrorFilter<CustomErrorFilter>();

var app = builder.Build();

app.UseRouting();
app.MapGraphQL();

app.Run();