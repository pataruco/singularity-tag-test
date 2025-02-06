using Libraries.Dynamics.DynamicsClient.Models;
using Microsoft.Identity.Client;

namespace Libraries.Dynamics.DynamicsClient.Factories;

public interface IConfidentialClientApplicationAdapter
{
    Task<AuthenticationResult> AcquireTokenForClientAsync(string[] scopes, CancellationToken cancellationToken);
}