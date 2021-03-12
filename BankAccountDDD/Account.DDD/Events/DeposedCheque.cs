using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class DeposedCheque : Event
    {
        public readonly Guid AccountId;
        public readonly decimal Funds;
        public DeposedCheque(Guid accountId, decimal funds)
        {
            AccountId = accountId;
            Funds = funds;
        }
    }
}
