using Grpc.Core;
using Libraries.Nba.GrpcContracts.V1;

namespace Application.Nba.Api.Services.V1;

public class GreeterService : GreeterV1.GreeterV1Base
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello, {request.Name}!"
        });
    }

    public override Task<GoodbyeReply> SayGoodbye(GoodbyeRequest request, ServerCallContext context)
    {
        return Task.FromResult(new GoodbyeReply
        {
            Message = $"Goodbye, {request.Name}!"
        });
    }
}