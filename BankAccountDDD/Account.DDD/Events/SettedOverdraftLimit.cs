using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class SettedOverdraftLimit : Event
    {
        public readonly Guid AccountId;
        public readonly double NewLimit;
        public SettedOverdraftLimit(Guid id, double newLimit)
        {
            AccountId = id;
            NewLimit = newLimit;
        }
    }
}
