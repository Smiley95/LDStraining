using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD.Commands
{
    public class CreateAccount : Command
    {
        public Guid AccountId;
        public string HolderName;
        public CreateAccount(Guid id, string holderName)
        {
            AccountId = id;
            HolderName = holderName;
        }
    }
}
