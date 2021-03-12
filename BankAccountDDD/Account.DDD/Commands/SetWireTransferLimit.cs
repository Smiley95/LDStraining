using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Commands
{
    public class SetWireTransferLimit : Command
    {
        public Guid AccountId;
        public decimal Limit;

        public SetWireTransferLimit(Guid id, decimal limit)
        {
            AccountId = id;
            Limit = limit;
        }
    }
}
