using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class Blocked : Event
    {
        public Guid BlockedAccount;
        public Blocked(Guid id)
        {
            BlockedAccount = id;
        }
    }
}
