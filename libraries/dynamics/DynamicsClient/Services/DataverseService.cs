using Libraries.Dynamics.DynamicsClient.Factories;
using Libraries.Dynamics.DynamicsClient.Models;
using Libraries.Dynamics.DynamicsClient.Utilities;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace Libraries.Dynamics.DynamicsClient.Services;

public class DataverseService : IDataverseService
{
    private readonly IOrganizationServiceAsync2 _serviceClient;
    private readonly ILogger<DataverseService> _logger;

    public DataverseService(IServiceClientFactory serviceClientFactory, ITokenProvider tokenProvider, ILogger<DataverseService> logger, IOptions<DynamicsOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        var baseUrl = options?.Value.BaseUrl ?? throw new ArgumentNullException(nameof(options));
        _serviceClient = serviceClientFactory.CreateServiceClient(new Uri(baseUrl), tokenProvider.GetAccessToken, true, _logger);
    }

    public async Task<WhoAmIResponse> GetWhoAmIAsync(CancellationToken cancellationToken = default)
    {
        var request = new WhoAmIRequest();
        var response = (WhoAmIResponse)await _serviceClient.ExecuteAsync(request, cancellationToken);

        return response;
    }
}