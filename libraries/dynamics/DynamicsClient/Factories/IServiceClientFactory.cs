using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace Libraries.Dynamics.DynamicsClient.Factories;

public interface IServiceClientFactory
{
    IOrganizationServiceAsync2 CreateServiceClient(Uri instanceUrl, Func<string, Task<string>> tokenProviderFunction, bool useUniqueInstance = true, ILogger logger = null);
}