using Orleans.MultiClient.DependencyInjection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrleansMultiClient(this IServiceCollection services, string serviceId, string clusterId, Action<IMultiClientBuilder> startup)
        {
            var build = new MultiClientBuilder(serviceId, clusterId, services);
            startup.Invoke(build);
            build.Build();
            return services;
        }
    }
}
