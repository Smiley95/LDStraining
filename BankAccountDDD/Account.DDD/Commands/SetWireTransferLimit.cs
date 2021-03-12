using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Commands
{
    public class SetWireTransferLimit : Command
    {
        public Guid AccountId;
        public double Limit;

        public SetWireTransferLimit(Guid id, double limit)
        {
            AccountId = id;
            Limit = limit;
        }
    }
}
