using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Events
{
    public class AccountCreated : Event
    {
        public readonly string HolderName;
        public Guid AccountId { get; set; }

        public AccountCreated(Guid accountId, string holderName)
        {
            AccountId = accountId;
            HolderName = holderName;
        }
    }
}
