using System;
using System.Collections.Generic;
using System.Text;
using PublicCommands = Accounts.Domain.Commands.Public;
using Accounts.Domain.Commands.Private;
using Linedata.Foundation.CommandReceiver;
using Linedata.Foundation.Messaging;

namespace CommandReceiver
{
    class CommandTranslatorToPublic : ITranslatorToPublicEx
    {
        public bool TranslateToPublic(Message privateMessage, out PublicResponse publicMessage)
        {
            switch (privateMessage.Payload)
            {
                case CreateAccount privateResponse:
                    publicMessage = new PublicResponse( 
                        new PublicCommands.CreateAccount(privateResponse.Amount),
                        privateMessage.Headers);
                    return true;
                case CreditAccount privateResponse:
                    publicMessage = new PublicResponse(
                        new PublicCommands.CreditAccount(privateResponse.AccountId,privateResponse.Amount),
                        privateMessage.Headers);
                    return true;
                case DebitAccount privateResponse:
                    publicMessage = new PublicResponse(
                        new PublicCommands.DebitAccount(privateResponse.AccountId, privateResponse.Amount),
                        privateMessage.Headers);
                    return true;

                default:
                    publicMessage = default;
                    return false;
            }
        }
    }
}
