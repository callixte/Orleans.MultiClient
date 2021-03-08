using Microsoft.Extensions.DependencyInjection;
using Orleans.MultiClient.DependencyInjection;
using System;
using Orleans.Hosting;

namespace Microsoft.Extensions.Hosting
{
    public static class GenericHostExtensions
    {
        public static IHostBuilder UseOrleansMultiClient(this IHostBuilder hostBuilder, string serviceId, string clusterId, Action<IMultiClientBuilder> startup)
        {
            hostBuilder.ConfigureServices((context, service) => service.AddOrleansMultiClient(serviceId, clusterId, startup));
            return hostBuilder;
        }
        
        public static ISiloHostBuilder AddOrleansMultiClient(this ISiloHostBuilder hostBuilder, string serviceId, string clusterId, Action<IMultiClientBuilder> startup)
        {
            hostBuilder.ConfigureServices((context, service) => service.AddOrleansMultiClient(serviceId, clusterId, startup));
            return hostBuilder;
        }
    }
}
