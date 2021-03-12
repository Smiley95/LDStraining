using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class WireTransferred : Event
    {
        public readonly Guid AccountId;
        public readonly double TransferredFunds;

        public WireTransferred(Guid accountId, double transferredFunds)
        {
            AccountId = accountId;
            TransferredFunds = transferredFunds;
        }
    }
}
