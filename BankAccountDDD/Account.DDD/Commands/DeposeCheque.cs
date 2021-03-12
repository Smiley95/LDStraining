using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Commands
{
    public class DeposeCheque : Command
    {
        public Guid AccountId;
        public decimal Funds;
        public DeposeCheque(Guid id, decimal funds)
        {
            AccountId = id;
            Funds = funds;
        }
    }
}
