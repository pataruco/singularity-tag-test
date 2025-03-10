using HotChocolate.Execution;
using JustDirect.Applications.CustomerApi.Queries;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.NUnit;

namespace JustDirect.Applications.CustomerApi.IntegrationTests;

public class ApiSchemaTests
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