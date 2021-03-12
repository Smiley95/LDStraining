using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Commands
{
    public class UnblockAccount : Command
    {
        public Guid AccountId;
        public UnblockAccount(Guid id)
        {
            AccountId = id;
        }
    }
}
