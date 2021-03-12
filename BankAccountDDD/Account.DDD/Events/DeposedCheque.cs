using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class DeposedCheque : Event
    {
        public readonly Guid AccountId;
        public readonly double Funds;
        public DeposedCheque(Guid accountId, double funds)
        {
            AccountId = accountId;
            Funds = funds;
        }
    }
}
