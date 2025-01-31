using Microsoft.Identity.Client;

namespace Libraries.Dynamics.DynamicsClient.Services;

public interface ITokenService
{
    Task<AuthenticationResult> GetAccessTokenAsync(CancellationToken cancellationToken = default);
}
