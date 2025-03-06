using Application.Api.Queries;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.NUnit;

namespace Application.Api.IntegrationTest;

public class Schema
{
    [Test]
    public void SchemaChangeTest()
    {
        // arrange
        var schema = new ServiceCollection()
            .AddGraphQLServer()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddGlobalObjectIdentification()
            .BuildSchemaAsync();
        schema.ToString().MatchSnapshot();
    }
}