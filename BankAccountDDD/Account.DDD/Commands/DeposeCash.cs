using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Commands
{
    public class DeposeCash : Command
    {
        public Guid AccountId;
        public decimal Funds;
        public DeposeCash(Guid id, decimal funds)
        {
            AccountId = id;
            Funds = funds;
        }
    }
}
