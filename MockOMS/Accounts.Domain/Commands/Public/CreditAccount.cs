using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CommandReceiver.Commands;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Accounts.Domain.Commands.Public
{
    public class CreditAccount : IValidate, IMessage<CreditAccount>
    {
        public string AccountId;
        public decimal Amount;

        public MessageDescriptor Descriptor => null;

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

        public CreditAccount Clone()
        {
            return new CreditAccount(this);
        }

        public bool Equals([AllowNull] CreditAccount other)
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

        public void MergeFrom(CreditAccount other)
        {
            if (other == null)
            {
                return;
            }

            Amount = other.Amount;
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
