using Grpc.Net.Client;
using JustDirect.Applications.Core.Services.Interfaces;
using JustDirect.Applications.CustomerApi.Helpers;
using JustDirect.Applications.CustomerApi.Inputs;
using JustDirect.Applications.CustomerApi.Types;
using JustDirect.Applications.Domain.Entities;
using Libraries.Nba.GrpcContracts.V1;

namespace JustDirect.Applications.CustomerApi.Queries;

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