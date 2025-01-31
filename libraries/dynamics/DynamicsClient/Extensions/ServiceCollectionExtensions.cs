using Libraries.Dynamics.DynamicsClient.Factories;
using Libraries.Dynamics.DynamicsClient.Models;
using Libraries.Dynamics.DynamicsClient.Services;
using Libraries.Dynamics.DynamicsClient.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Libraries.Dynamics.DynamicsClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDynamicsClient(this IServiceCollection services, IConfiguration configuration, string configSection = "DynamicsClient")
        {
            services.Configure<DynamicsOptions>(configuration.GetSection(configSection));
            services.AddLogging();
            services.AddSingleton<IConfidentialClientApplicationAdapter, ConfidentialClientApplicationAdapter>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<ITokenProvider, TokenProvider>();
            services.AddScoped<IDataverseService, DataverseService>();
            services.AddScoped<IServiceClientFactory, ServiceClientFactory>();
            return services;
        }
    }
}
