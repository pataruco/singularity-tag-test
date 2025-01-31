
using Libraries.Dynamics.DynamicsClient.Services;

namespace Libraries.Dynamics.DynamicsClient.Utilities;

public class TokenProvider(ITokenService tokenService) : ITokenProvider
{
    public async Task<string> GetAccessToken(string instanceUri)
    {
        var token = await tokenService.GetAccessTokenAsync();
        return token.AccessToken;
    }

    public async Task<HttpRequestMessage> AddAuthorization(HttpRequestMessage message, CancellationToken cancellationToken = default)
    {
        var response = await tokenService.GetAccessTokenAsync(cancellationToken);

        message.Headers.TryAddWithoutValidation("Authorization", string.Join(" ", response?.TokenType, response?.AccessToken));

        return message;
    }
}