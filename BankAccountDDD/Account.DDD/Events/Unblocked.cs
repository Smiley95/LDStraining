using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class Unblocked : Event
    {
        public Guid UnblockedAccount;
        public Unblocked(Guid unblockedAccount)
        {
            UnblockedAccount = unblockedAccount;
        }
    }
}
