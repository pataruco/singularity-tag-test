using Libraries.Dynamics.DynamicsClient.Models;
using Libraries.Dynamics.DynamicsClient.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace Libraries.Dynamics.DynamicsClient.Factories
{
    public class DataverseContextFactory : IDataverseContextFactory
    {
        private readonly IOrganizationServiceAsync2 _serviceClient;
        public DataverseContextFactory(IServiceClientFactory serviceClientFactory, ITokenProvider tokenProvider, ILogger<DataverseContext> logger,
            IOptions<DynamicsOptions> options)
        {
            var _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            var baseUrl = options?.Value.BaseUrl ?? throw new ArgumentNullException(nameof(options));
            _serviceClient = serviceClientFactory.CreateServiceClient(new Uri(baseUrl), tokenProvider.GetAccessToken, true, _logger);
        }
        public DataverseContext CreateDataverseContext()
        {
            return new DataverseContext(_serviceClient);
        }
    }
}