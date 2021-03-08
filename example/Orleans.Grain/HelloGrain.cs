using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Grains2;

namespace Orleans.Grains
{
    /// <summary>
    /// Orleans grain implementation class HelloGrain.
    /// </summary>
    public class HelloGrain : Grain, IHelloA
    {
        private readonly ILogger logger;
        private readonly IServiceProvider services;

        public HelloGrain(ILogger<HelloGrain> logger, IServiceProvider services)
        {
            this.logger = logger;
            this.services = services;
        }

       public async Task<string> SayHello(string greeting)
        {
            logger.LogInformation($"SayHello message received: greeting = '{greeting}'");
            var grainB = services.GetRequiredService<IOrleansClient>().GetGrain<IHelloB>(1);
            var result = await grainB.SayHello("Hello from A");
            var grainA = services.GetRequiredService<IOrleansClient>().GetGrain<IHelloA>(12);
            var resultSelf = await grainA.TalkToMyself("hello you!");
            return $"You said: '{greeting}', I say: Hello!";
        }

       /// <inheritdoc />
       public async Task<string> TalkToMyself(string greeting)
       {
           logger.LogInformation($"TalkToMyself message received: greeting = '{greeting}'");
           return "I respond to myself";
       }
    }
}
