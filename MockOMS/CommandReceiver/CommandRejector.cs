using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.CommandReceiver;
using Linedata.Foundation.Messaging;
using Linedata.Gateway.Common;

namespace CommandReceiver
{
    public sealed class CommandRejector : ICommandRejectorEx
    {
        public Message RejectCommand(Message command, string reason)
        {
            var commandRejected = new CommandRejected { Reason = reason };
            return commandRejected.ToMessage();
        }
    }
}
