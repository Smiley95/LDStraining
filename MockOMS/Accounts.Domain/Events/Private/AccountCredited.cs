using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.EventSourcing;
using Linedata.Foundation.Domain.Messaging;

namespace Accounts.Domain.Events.Private
{
    public class AccountCredited : Event
    {
        public Identity AccountId;
        public decimal Amount;

        public AccountCredited(Identity accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
