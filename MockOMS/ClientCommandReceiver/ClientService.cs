using Accounts.Domain.Commands.Public;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Linedata.Foundation.Messaging;
using Accounts.Domain.Events.Public;
using Linedata.Foundation.Messaging.Endpoints.Duplex;

namespace ClientCommandReceiver
{
    class ClientService : BackgroundService
    {
        readonly IDuplexClientEndpoint _endpoint;

        public ClientService(
            IDuplexClientEndpointFactory endpointFactory)
        {
            _endpoint = endpointFactory.CreateDuplexClientEndpoint("CommandReceiverClientEndpoint");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting Command Receiver Client Service...");
            await _endpoint.ConnectAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                var amount = 120;
                var request = new CreateAccount(amount);

                Console.WriteLine("Sending command "+ request);

                var response = await _endpoint.RequestResponse
                    .SendRequestAsync<CreateAccount, AccountCreated>(request, cancellationToken: stoppingToken);

                Console.WriteLine("Received response " + response.ToJson());

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
