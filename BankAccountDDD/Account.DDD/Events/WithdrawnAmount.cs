using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class WithdrawnAmount : Event
    {
        public readonly Guid AccountId;
        public readonly double WithdrawnFunds;

        public WithdrawnAmount(Guid accountId, double withdrawnFunds)
        {
            AccountId = accountId;
            WithdrawnFunds = withdrawnFunds;
        }
    }
}
