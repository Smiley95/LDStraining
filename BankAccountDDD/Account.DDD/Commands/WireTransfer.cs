using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Commands
{
    public class WireTransfer : Command
    {
        public Guid AccountId;
        public double Funds;
        public WireTransfer(Guid id, double funds)
        {
            AccountId = id;
            Funds = funds;
        }
    }
}
