using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Commands
{
    public class BlockAccount : Command
    {
        public Guid AccountId;
        public BlockAccount(Guid id)
        {
            AccountId = id;
        }
    }
}
