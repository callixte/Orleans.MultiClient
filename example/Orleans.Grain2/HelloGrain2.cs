using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Orleans.Grains2
{
    /// <summary>
    /// Orleans grain implementation class HelloGrain.
    /// </summary>
    public class HelloGrain2 : Grain, IHelloB
    {
        private readonly ILogger logger;
        private readonly IServiceProvider services;

        public HelloGrain2(ILogger<HelloGrain2> logger, IServiceProvider services)
        {
            this.logger = logger;
            this.services = services;
        }

       public async Task<string> SayHello(string greeting)
        {
            var sp = services.GetRequiredService<IOrleansClient>().GetStreamProvider<IHelloB>("SMS");
            var stream = sp.GetStream<string>(Guid.Parse("a9b9234e-ef9e-4d88-9934-580d82ad2f1c"), "MyStream");
            await stream.OnNextAsync(greeting);
            
            logger.LogInformation($"SayHello message received: greeting = '{greeting}'");
            return $"You said: '{greeting}', I say: Hello!";
        }
    }
}
