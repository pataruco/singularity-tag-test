using Libraries.Dynamics.DynamicsClient.Factories;
using Libraries.Dynamics.DynamicsClient.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace Libraries.Dynamics.DynamicsClient.Services;

public class TokenService : ITokenService, IDisposable
{
    private readonly DynamicsOptions _options;
    private readonly ILogger<TokenService> _logger;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly IConfidentialClientApplicationAdapter _clientApplicationAdapter;

    private AuthenticationResult _cachedToken;
    private bool _disposed;

    public TokenService(IOptions<DynamicsOptions> options, IConfidentialClientApplicationAdapter clientApplicationAdapter, ILogger<TokenService> logger)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _clientApplicationAdapter = clientApplicationAdapter ?? throw new ArgumentNullException(nameof(clientApplicationAdapter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        ValidateOptions(_options);
    }

    public async Task<AuthenticationResult> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        
        try
        {
            if (!string.IsNullOrWhiteSpace(_cachedToken?.AccessToken) && DateTimeOffset.UtcNow < _cachedToken.ExpiresOn)
            {
                _logger.LogInformation("Returning cached token.");
                return _cachedToken;
            }
            
            var scopes = new[] { $"{_options.BaseUrl}/.default" };

            _cachedToken = await _clientApplicationAdapter.AcquireTokenForClientAsync(scopes, cancellationToken);

            _logger.LogInformation("Token acquired successfully.");
            return _cachedToken;
        }
        catch (MsalServiceException ex)
        {
            _logger.LogError(ex, "An error occurred while acquiring the token.");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        _semaphore?.Dispose();
        _disposed = true;
    }

    private static void ValidateOptions(DynamicsOptions options)
    {
        if (string.IsNullOrEmpty(options.ClientId))
            throw new ArgumentException("ClientId must be provided.", nameof(options.ClientId));
        if (string.IsNullOrEmpty(options.ClientSecret))
            throw new ArgumentException("ClientSecret must be provided.", nameof(options.ClientSecret));
        if (string.IsNullOrEmpty(options.TenantId))
            throw new ArgumentException("TenantId must be provided.", nameof(options.TenantId));
        if (string.IsNullOrEmpty(options.BaseUrl))
            throw new ArgumentException("BaseUrl must be provided.", nameof(options.BaseUrl));
        if (string.IsNullOrEmpty(options.AuthorityDomain))
            throw new ArgumentException("AuthorityDomain must be provided.", nameof(options.AuthorityDomain));
    }
}
