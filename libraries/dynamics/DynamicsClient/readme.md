# DynamicsClient

The `DynamicsClient` library provides a set of services and utilities to interact with Microsoft Dynamics 365. It includes functionality for token management, service client creation, and more.

## Features

- Token management using `TokenService`
- Service client factory for creating Dynamics 365 service clients
- Configuration options for Dynamics 365 settings

## Installation

To install the `DynamicsClient` library, add the following NuGet package to your project:
`dotnet add package DynamicsClient`

## Configuration

Add the following configuration section to your `appsettings.json`:
```
{
  "DynamicsClient": {
    "BaseUrl": "https://your-dynamics-instance.api.crm.dynamics.com/",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "TenantId": "your-tenant-id",
    "AuthorityDomain": "https://login.microsoftonline.com/"
  }
}
```

## Usage

### Registering Services

In your `Startup.cs` or `Program.cs`, register the DynamicsClient services with the dependency injection container:
```
using Libraries.Dynamics.DynamicsClient.Extensions; using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDynamicsClient(configuration);
    }
}
```

### Acquiring Tokens

To acquire an access token, use the `TokenService`:
```
using Libraries.Dynamics.DynamicsClient.Services; using Microsoft.Extensions.DependencyInjection;

public class MyService
{
    private readonly ITokenService _tokenService;
    public MyService(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var tokenResult = await _tokenService.GetAccessTokenAsync();
        return tokenResult.AccessToken;
    }
}
```

### Creating Service Clients

To create a Dynamics 365 service client, use the `ServiceClientFactory`:
```
using Libraries.Dynamics.DynamicsClient.Factories; using Microsoft.Extensions.DependencyInjection;
public class MyService { private readonly IServiceClientFactory _serviceClientFactory;

public MyService(IServiceClientFactory serviceClientFactory)
{
    _serviceClientFactory = serviceClientFactory;
}

public async Task UseServiceClientAsync()
{
    var serviceClient = await _serviceClientFactory.CreateServiceClientAsync();
    // Use the service client
}
```