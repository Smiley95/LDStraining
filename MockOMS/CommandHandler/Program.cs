using System;
using System.Threading.Tasks;
using Accounts.Domain.Commands.Public;
using Accounts.Domain.Events.Private;
using CommandReceiver;
using Linedata.Foundation.EventStorage;
using Linedata.Foundation.EventStorage.EventStore;
using Linedata.Foundation.Messaging;
using Linedata.Foundation.Messaging.Core;
using Linedata.Foundation.Messaging.Serialization.Json;
using Linedata.Foundation.Messaging.Transport.ZeroMq;
using Linedata.Foundation.ServiceHosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommandHandler
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
                            .AddSingleton<IRequestHandler<CreateAccount, AccountCreated>, RequestHandler>()
                            .AddHostedService<CommandHandlerService>()
                            .ConfigureEventStorage(h.Configuration);
                    })
                .Build()
                .RunAsync();
        }
    }
}
