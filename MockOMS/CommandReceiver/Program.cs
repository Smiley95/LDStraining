using Accounts.Domain.Commands.Public;
using Linedata.Foundation.CommandReceiver.Configuration;
using Linedata.Foundation.Messaging.Core;
using Linedata.Foundation.Messaging.Serialization.Json;
using Linedata.Foundation.Messaging.Steeltoe;
using Linedata.Foundation.Messaging.Transport.ZeroMq;
using Linedata.Foundation.ServiceHosting;
using System;
using System.Threading.Tasks;

namespace CommandReceiver
{


    static class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Command receiver is on!");
            await DefaultLinedataHostBuilder
                .Create(args)
                .ConfigureServices(
                    (b, services) =>
                    {
                        services
                            .AddMessaging()
                            .AddMessagingSteeltoe()
                            .AddZeroMqTransport()
                            .RegisterSampleMessages(new JsonNetSerializer())
                            .AddCommandReceiverService()
                            .RegisterMessages(
                                registerer =>
                                {
                                    registerer
                                        .Register<CreateAccount>()
                                        .Register<CreditAccount>()
                                        .Register<DebitAccount>();
                                })
                            .WithResponseTranslatorEx<CommandTranslatorToPublic>()
                            .WithRequestTranslatorEx<CommandTranslator>()
                            .WithCommandValidatorEx<CommandValidator>()
                            .WithCommandRejectorEx<CommandRejector>()
                            .WithDefaultDispatcher();
                    })
                .Build()
                .StartAsync();
        }
    }
}
