using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Grains;
using Orleans.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Grains2;

namespace Orleans.Host
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ServiceId = "A";
                    options.ClusterId = "AApp";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .AddOrleansMultiClient("A", "AApp", build =>
                {
                    build.AddClient(opt =>
                    {
                        opt.ServiceId = "A";
                        opt.ClusterId = "AApp";
                        opt.SetServiceAssembly(typeof(IHelloA).Assembly);
                        opt.Configure = (b =>
                        {
                            b.UseLocalhostClustering();
                        });
                    });
                    build.AddClient(opt =>
                    {
                        opt.ServiceId = "B";
                        opt.ClusterId = "BApp";
                        opt.SetServiceAssembly(typeof(IHelloB).Assembly);
                        opt.Configure = (b =>
                        {
                            b.UseLocalhostClustering(gatewayPort: 30001);
                            b.AddSimpleMessageStreamProvider("SMS");
                        });
                    });
                });

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
