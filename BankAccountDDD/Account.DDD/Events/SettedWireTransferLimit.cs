using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class SettedWireTransferLimit : Event
    {
        public readonly Guid AccountId;
        public readonly double NewLimit;
        public SettedWireTransferLimit(Guid id, double newLimit)
        {
            AccountId = id;
            NewLimit = newLimit;
        }
    }
}
