using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Accounts.Domain.Commands;
using Linedata.Foundation.Domain.Messaging;
using Microsoft.Extensions.Options;
using ProtoBuf;

namespace Accounts.Domain.Commands.Private
{
    public class CreateAccount : Command
    {
        public decimal Amount;

        public CreateAccount(decimal amount)
        {
            Amount = amount;
        }
        public CreateAccount(CreateAccount cmd)
        {
            Amount = cmd.Amount;
        }
    }
}
