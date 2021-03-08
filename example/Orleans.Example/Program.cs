using Microsoft.Extensions.DependencyInjection;
using Orleans.Grains;
using Orleans.Grains2;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;
using Orleans.Streams;

namespace Orleans.Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter btn start");
            Console.ReadKey();
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddOrleansMultiClient("", "", build =>
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

            var sp = services.BuildServiceProvider();
            
            var streamprovider = sp.GetRequiredService<IOrleansClient>().GetStreamProvider<IHelloB>("SMS");
            var stream = streamprovider.GetStream<string>(Guid.Parse("a9b9234e-ef9e-4d88-9934-580d82ad2f1c"), "MyStream");
            await stream.SubscribeAsync<string>(async (data, token) => Console.WriteLine($"From stream: {data}"));

            var service = sp.GetRequiredService<IOrleansClient>().GetGrain<IHelloA>(1);
            var result1 = service.SayHello("Hello World Success Grain1").GetAwaiter().GetResult();


            var serviceA = sp.GetRequiredService<IOrleansClient>().GetClusterClient(typeof(IHelloA).Assembly) .GetGrain<IHelloA>(1);
            var resultA1 = serviceA.SayHello("Hello World Success GrainA").GetAwaiter().GetResult();


            var service2 = sp.GetRequiredService<IOrleansClient>().GetGrain<IHelloB>(1);
            var result2 = service2.SayHello("Hello World Success Grain2").GetAwaiter().GetResult();
            Console.WriteLine("dev1:" + result1);
            Console.WriteLine("dev2:" + result2);

            Console.ReadKey();
        }
    }
}
