using Linedata.Foundation.ServiceHosting;
using System;
using System.Threading.Tasks;
using Accounts.Domain;
using Linedata.Foundation.Messaging;
using Linedata.Foundation.Messaging.Core;
using Linedata.Foundation.Messaging.Endpoints.Duplex;
using Linedata.Foundation.Messaging.Serialization.Protobuf;
using Linedata.Foundation.Messaging.Transport.ZeroMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventSubscriber
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var host = DefaultLinedataHostBuilder.Create(args)
                .ConfigureServices(
                    services =>
                    {
                        services.AddMessagingCore();
                        services.AddZeroMqTransport();
                        services.AddSerializers(msb => msb.AddProtobufSerializer(MessageCodes.TypeToCode));
                    })
                .Build();

            await host.StartAsync();

            Console.WriteLine("Press any key to start subscribing");
            Console.ReadKey(true);

            var clientEndpointFactory = host.Services.GetRequiredService<IDuplexClientEndpointFactory>();
            var clientEndpoint = clientEndpointFactory.CreateDuplexClientEndpoint("ConsoleClient");
            await clientEndpoint.ConnectAsync();

            var eventHandler = new ClientEventHandler();
            var subscription = await clientEndpoint.PubSub.SubscribeAsync(@"\Test", 0, eventHandler);

            Console.WriteLine("Press any key to stop");
            while (true)
            {
                if (!Console.KeyAvailable)
                    await Task.Delay(100);

                Console.ReadKey(true);
                break;
            }

            await subscription.StopAsync();
            await clientEndpoint.DisconnectAsync();

            await host.StopAsync();
        }
    }
}
