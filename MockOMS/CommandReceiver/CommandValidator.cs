using System;
using System.Collections.Generic;
using System.Text;
using CommandReceiver.Commands;
using Linedata.Foundation.CommandReceiver;
using Linedata.Foundation.Messaging;

namespace CommandReceiver
{
    class CommandValidator : ICommandValidatorEx
    {
        public bool ValidateCommand(Message command, out string reason)
        {

            if (!System.Guid.TryParse(command.Headers.FindHeader("AncestorId"), out var ancestorId) ||
                ancestorId == System.Guid.Empty)
            {
                reason = $"AncestorId must be a valid Guid. {command.Payload.GetType().Name} not valid!";
                return false;
            }
            if (!(command.Payload is IValidate canValidateCommand))
            {
                reason = $"{command.GetType().Name} is not an IValidate. it cannot be validated";
                return false;
            }

            var isValid = canValidateCommand.TryValidate(out var error);

            reason = error;
            return isValid;
        }
    }
}
