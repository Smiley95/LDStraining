using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Commands
{
    public class DeposeCheque : Command
    {
        public Guid AccountId;
        public double Funds;
        public DeposeCheque(Guid id, double funds)
        {
            AccountId = id;
            Funds = funds;
        }
    }
}
