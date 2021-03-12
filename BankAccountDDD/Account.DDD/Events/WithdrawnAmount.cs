using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class WithdrawnAmount : Event
    {
        public readonly Guid AccountId;
        public readonly decimal WithdrawnFunds;

        public WithdrawnAmount(Guid accountId, decimal withdrawnFunds)
        {
            AccountId = accountId;
            WithdrawnFunds = withdrawnFunds;
        }
    }
}
