using Accounts.Domain.Commands.Public;
using Accounts.Domain.Events.Private;
using Linedata.Foundation.Messaging;
using System;
using System.Threading.Tasks;

namespace CommandHandler
{
    class RequestHandler : IRequestHandler<CreateAccount, AccountCreated>
    {
        public Task<AccountCreated> HandleAsync(CreateAccount request)
        {
            var response = new AccountCreated(Guid.NewGuid(), request.Amount);
            return Task.FromResult(response);
        }
    }
}
