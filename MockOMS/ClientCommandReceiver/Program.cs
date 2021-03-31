using Linedata.Foundation.Messaging.Core;
using Linedata.Foundation.Messaging.Serialization.Json;
using Linedata.Foundation.Messaging.Transport.ZeroMq;
using Linedata.Foundation.ServiceHosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ClientCommandReceiver
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            await DefaultLinedataHostBuilder
                .Create(args)
                .ConfigureServices(
                    (h, services) =>
                    {
                        services
                            .AddMessaging()
                            .AddZeroMqTransport()
                            .RegisterSampleMessages(new JsonNetSerializer())
                            .AddHostedService<ClientService>();
                    })
                .Build()
                .RunAsync();
        }
    }
}
