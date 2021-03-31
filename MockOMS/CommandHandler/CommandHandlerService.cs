using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Domain.Commands.Public;
using Accounts.Domain.Events.Private;
using Linedata.Foundation.Messaging;
using Linedata.Foundation.Messaging.Endpoints.Duplex;
using Microsoft.Extensions.Hosting;

namespace CommandHandler
{
    class CommandHandlerService : BackgroundService
    {
        readonly IDuplexServerEndpoint _endpoint;
        readonly IRequestHandler<CreateAccount, AccountCreated> _requestHandler;

        public CommandHandlerService(
            IDuplexServerEndpointFactory endpointFactory, IRequestHandler<CreateAccount, AccountCreated> requestHandler)
        {
            _requestHandler = requestHandler;

            _endpoint = endpointFactory.CreateDuplexServerEndpoint(
                "HandlerServerEndpoint",
                OnConnected,
                null);
            Console.WriteLine( "endpoint output " , _endpoint.ToJson());
        }

        Task OnConnected(IDuplexServerEndpoint endpoint, IDuplexConnectionEndpoint connection)
        {
            connection.RequestResponse.SetHandler(_requestHandler);

            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting Command Handler Service...");

            await _endpoint.OpenAsync(stoppingToken);


            await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
        }

        public override void Dispose()
        {
            base.Dispose();

            _endpoint.Dispose();
        }
    }
}
