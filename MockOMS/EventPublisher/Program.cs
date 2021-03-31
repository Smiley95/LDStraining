using Linedata.Foundation.ServiceHosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace EventPublisher
{
    [ExcludeFromCodeCoverage]
    static class Program
    {
        static async Task Main(string[] arguments)
        {
            Console.Title = "Linedata Gateway Event Publisher Service";

            try
            {
                var host = DefaultLinedataHostBuilder
                    .Create(arguments)
                    .ConfigureServices((context, services) => services.AddGatewayEventPublisher(context.Configuration))
                    .Build();

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error encountered: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                Environment.Exit(1);
            }
        }
    }
}
