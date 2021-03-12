using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class AccountCreated : Event
    {
        public readonly string HolderName;
        public readonly Guid AccountId;

        public AccountCreated(Guid id, string holderName)
        {
            AccountId = id;
            HolderName = holderName;
        }
    }
}
