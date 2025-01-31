using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace Libraries.Dynamics.DynamicsClient.Factories;

public class ServiceClientFactory : IServiceClientFactory
{
    public IOrganizationServiceAsync2 CreateServiceClient(Uri instanceUrl, Func<string, Task<string>> tokenProviderFunction,
        bool useUniqueInstance = true, ILogger logger = null)
    {
        var serviceClient = new ServiceClient(instanceUrl, tokenProviderFunction, useUniqueInstance, logger);

        if (!serviceClient.IsReady)
        {
            throw new InvalidOperationException($"Failed to create ServiceClient: {serviceClient.LastError}");
        }

        return serviceClient;
    }
}