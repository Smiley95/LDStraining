using System;
using System.Collections.Generic;
using System.Text;
using Accounts.Domain;
using CommandReceiver.Commands;
using Linedata.Foundation.CommandReceiver;
using Linedata.Foundation.Messaging;
using PrivateCommand = Accounts.Domain.Commands.Private;

namespace CommandReceiver
{
    class CommandTranslator : ITranslatorToPrivateEx
    {
        public bool TranslateToPrivate(Message publicMessage, out AddressedPrivateCommand privateMessage)
        {
            bool result;

            try
            {
                result = TranslateToPrivateCore(publicMessage, out privateMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR Translator input [public]:" + publicMessage);
            }

            if (result)
            {
                Console.WriteLine("Translator input [public]: " + publicMessage);
                return true;
            }

            Console.WriteLine("FAILED Translator input [public]: " + publicMessage);
            return false;
        }

        static bool TranslateToPrivateCore(Message publicMessage, out AddressedPrivateCommand privateMessage)
        {
            switch (publicMessage.Payload)
            {
                case CreateAccount cmd:
                    privateMessage = CreateFromTranslation(
                        new PrivateCommand.CreateAccount(cmd.Amount),
                        publicMessage.Headers,
                        "HandlerClientEndpoint");
                    return true;
                case CreditAccount cmd:
                    privateMessage = CreateFromTranslation(
                        new PrivateCommand.CreditAccount(cmd.AccountId,cmd.Amount),
                        publicMessage.Headers,
                        "HandlerClientEndpoint");
                    return true;
                case DebitAccount cmd:
                    privateMessage = CreateFromTranslation(
                        new PrivateCommand.DebitAccount(cmd.AccountId, cmd.Amount),
                        publicMessage.Headers,
                        "HandlerClientEndpoint");
                    return true;
                default:
                    privateMessage = null;
                    return false;
            }
        }

        static AddressedPrivateCommand CreateFromTranslation<T>(
            T translated,
            MessageHeaders headers,
            string destination)
        {
            return new AddressedPrivateCommand(
                translated,
                destination,
                headers);
        }

    }

}
