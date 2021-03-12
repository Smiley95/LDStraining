using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class WireTransferred : Event
    {
        public readonly Guid AccountId;
        public readonly decimal TransferredFunds;

        public WireTransferred(Guid accountId, decimal transferredFunds)
        {
            AccountId = accountId;
            TransferredFunds = transferredFunds;
        }
    }
}
