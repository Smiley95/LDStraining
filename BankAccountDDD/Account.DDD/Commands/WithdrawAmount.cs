using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Commands
{
    public class WithdrawAmount : Command
    {
        public Guid AccountId;
        public decimal Funds;
        public WithdrawAmount(Guid id, decimal funds)
        {
            AccountId = id;
            Funds = funds;
        }
    }
}
