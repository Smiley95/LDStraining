using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CommandReceiver.Commands;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Accounts.Domain.Commands.Public
{
    public class DebitAccount : IValidate, IMessage<DebitAccount>

    {
        public string AccountId;
        public decimal Amount;

        public DebitAccount(string accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
        public DebitAccount(DebitAccount cmd)
        {
            AccountId = cmd.AccountId;
            Amount = cmd.Amount;
        }
        public MessageDescriptor Descriptor => null;

        public int CalculateSize()
        {
            int size = 0;
            if (AccountId != null)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(AccountId);
            }
            if (Amount != 0)
            {
                size += 1 + CodedOutputStream.ComputeDoubleSize((double)Amount);
            }
            return size;
        }

        public DebitAccount Clone()
        {
            return new DebitAccount(this);
        }

        public bool Equals([AllowNull] DebitAccount other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return AccountId != other.AccountId;
        }

        public void MergeFrom(DebitAccount message)
        {
            if (message == null)
            {
                return;
            }

            AccountId = message.AccountId;
            Amount = message.Amount;
        }

        public void MergeFrom(CodedInputStream input)
        {
            input.ReadRawMessage(this);
        }

        public bool TryValidate(out string error)
        {
            const string commonError = " is invalid input";
            var errorsText = new StringBuilder();

            if (Amount <= 0)
            {
                errorsText.Append($"{Amount}{commonError}.");
            }

            error = errorsText.Length > 0 ? errorsText.ToString() : null;
            return error == null;
        }

        public void WriteTo(CodedOutputStream output)
        {
            output.WriteRawMessage(this);
        }
    }
}
