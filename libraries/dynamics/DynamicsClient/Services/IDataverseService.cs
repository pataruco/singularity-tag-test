using Microsoft.Crm.Sdk.Messages;

namespace Libraries.Dynamics.DynamicsClient.Services;

public interface IDataverseService
{
    Task<WhoAmIResponse> GetWhoAmIAsync(CancellationToken cancellationToken = default);
}