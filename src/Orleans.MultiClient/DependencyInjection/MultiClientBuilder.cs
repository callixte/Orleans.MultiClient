using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime;
using System;
using System.Collections.Generic;

namespace Orleans.MultiClient.DependencyInjection
{
    public class MultiClientBuilder : IMultiClientBuilder
    {
        public string LocalServiceId { get;  }
        
        public string LocalClusterId { get; }
        
        public IServiceCollection Services { get; }
        public Action<IClientBuilder> OrleansConfigure { get; set; }
        public IList<OrleansClientOptions> ClientOptions { get; set; } = new List<OrleansClientOptions>();
        public MultiClientBuilder(string serviceId, string clusterId, IServiceCollection services)
        {
            LocalServiceId = serviceId;
            LocalClusterId = clusterId;
            Services = services;
        }
        public void Build()
        {
            if (this.ClientOptions.Count <= 0)
            {
                throw new ArgumentNullException($"Please add silo via MultiClientBuilderExtensions.AddClient");
            }
            foreach (var client in this.ClientOptions)
            {
                if (client.Configure == null)
                    client.Configure = this.OrleansConfigure;
                if (client.ServiceList.Count == 0)
                    throw new ArgumentNullException($"Request to go to the configuration OrleansClientOptions.SetServiceAssembly Orleans interface");
                foreach (var serviceName in client.ServiceList)
                {
                    //if (!client.ExistAssembly(serviceName))
                    //    throw new ArgumentNullException($"{serviceName} service does not exist in the assembly");

                    Services.AddSingletonNamedService<IClusterClientBuilder>(serviceName, (sp, key) 
                        => new ClusterClientBuilder(sp, client, key, client.ServiceId == LocalServiceId && client.ClusterId == LocalClusterId));
                }
            }

            this.Services.AddSingleton(typeof(IKeyedServiceCollection<,>), typeof(KeyedServiceCollection<,>));
            this.Services.AddSingleton<IClusterClientFactory, MultiClusterClientFactory>();
            this.Services.AddSingleton<IOrleansClient, OrleansClient>();
        }
    }
}
