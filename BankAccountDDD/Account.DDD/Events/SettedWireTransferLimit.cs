using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class SettedWireTransferLimit : Event
    {
        public readonly Guid AccountId;
        public readonly decimal NewLimit;
        public SettedWireTransferLimit(Guid id, decimal newLimit)
        {
            AccountId = id;
            NewLimit = newLimit;
        }
    }
}
