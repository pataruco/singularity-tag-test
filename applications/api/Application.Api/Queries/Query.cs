using Application.Api.Helpers;
using Application.Api.Inputs;
using Application.Api.Types;
using Application.Core.Services.Interfaces;
using Application.Domain.Entities;
using Grpc.Net.Client;
using Libraries.Nba.GrpcContracts.V1;

namespace Application.Api.Queries;

public class Query
{
    public CustomerType? GetCustomer(CustomerWhereUniqueInput where, [Service] ICustomerService customerService)
    {
        Customer? result = CustomerHelper.GetCustomerById(where, customerService);

        return result == null ? null : new CustomerType(result);

    }

    public async Task<string> Test(HelloRequest request)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:8081");
        var client = new GreeterV1.GreeterV1Client(channel);
        var reply = await client.SayHelloAsync(request);

        return reply.Message;
    }
}