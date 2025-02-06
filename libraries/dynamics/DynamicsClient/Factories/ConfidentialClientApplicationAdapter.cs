using Libraries.Dynamics.DynamicsClient.Models;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace Libraries.Dynamics.DynamicsClient.Factories;

public class ConfidentialClientApplicationAdapter : IConfidentialClientApplicationAdapter
{
    private readonly DynamicsOptions _options;

    public ConfidentialClientApplicationAdapter(IOptions<DynamicsOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<AuthenticationResult> AcquireTokenForClientAsync(string[] scopes,
        CancellationToken cancellationToken)
    {
        var confidentialClient = ConfidentialClientApplicationBuilder.Create(_options.ClientId)
            .WithClientSecret(_options.ClientSecret)
            .WithAuthority(new Uri($"{_options.AuthorityDomain}{_options.TenantId}"))
            .Build();

        return await confidentialClient.AcquireTokenForClient(scopes).ExecuteAsync(cancellationToken);
    }
}