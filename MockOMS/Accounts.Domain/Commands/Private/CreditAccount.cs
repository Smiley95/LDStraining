using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Linedata.Foundation.Domain.Messaging;

namespace Accounts.Domain.Commands.Private
{
    public class CreditAccount : Command
    {
        public string AccountId;
        public decimal Amount;

        public CreditAccount(string accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
        public CreditAccount(CreditAccount cmd)
        {
            AccountId = cmd.AccountId;
            Amount = cmd.Amount;
        }
    }
}
