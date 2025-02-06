using System.Threading;

namespace Libraries.Dynamics.DynamicsClient.Utilities;

public interface ITokenProvider
{
    Task<string> GetAccessToken(string instanceUri);
    Task<HttpRequestMessage> AddAuthorization(HttpRequestMessage message, CancellationToken cancellationToken);
}